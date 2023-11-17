using AutoMapper;
using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Filters;
using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.DTOs.Reviews;
using HotelBooking.Application.DTOs.RoomTypes;
using HotelBooking.Application.DTOs.Sorting;
using HotelBooking.Application.Enums.Filters;
using HotelBooking.Application.Enums.Sorting;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Application.Specifications;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace HotelBooking.Infrastructure.Services;
public class HotelService : IHotelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public HotelService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<HotelSearchFilterResponse> GetHotelsAsync(HotelSearchRequest request)
    {
        var checkInDate = request.SearchCriteria.CheckInDate;
        var checkOutDate = request.SearchCriteria.CheckOutDate;
        var pageIndex = request.Page.PageIndex;
        var pageSize = request.Page.PageSize;

        if (!await _unitOfWork.Repository<City>().ExistsByAsync(_ => _.Id == request.CityId))
        {
            throw new NotFoundException(nameof(City), request.CityId);
        }

        //var paginatedHotel = await _unitOfWork.Repository<Hotel>()
        //     .FindAsync<HotelResponse>(
        //        configuration: _mapper.ConfigurationProvider,
        //        pageIndex: pageIndex,
        //        pageSize: pageSize,
        //        expressionHotelArea: h =>
        //                    h.Address.CityId == request.CityId &&
        //                    h.RoomTypes.Any(
        //                        r => r.ReservationDetails.Where(rd =>
        //                               checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
        //                               checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
        //                               rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
        //                               rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
        //                             .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability));

        var specification = new HotelSearchWithSpecification(request);

        var paginatedHotel = await _unitOfWork.Repository<Hotel>()
            .FindAsync<HotelResponse>(
                _mapper.ConfigurationProvider,
                pageIndex,
                pageSize,
                specification.Criteria,
                specification.OrderBy);

        var roomTypes = await _unitOfWork.Repository<RoomType>()
            .FindAsync(
                r => paginatedHotel.Select(_ => _.Id).Contains(r.HotelId) &&
                r.ReservationDetails.Where(rd =>
                    rd.Reservation.Status != ReservationStatus.Canceled &&
                   (checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate))
                .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability);

        paginatedHotel.ForEach(hotel =>
        {
            hotel.IsSoldOut = !roomTypes.Any(_ => _.HotelId == hotel.Id);
            hotel.PricePerNight = roomTypes.Where(_ => _.HotelId == hotel.Id).MinBy(_ => _.Price)?.Price;
        });
        return new()
        {
            Data = await paginatedHotel.ToPaginatedResponseAsync(),
            SortMatrix = await CreateSortMatrix(),
            MatrixGroupFilters = await UpdateMatrixGroup(request)
        };
    }

    protected async Task<List<MatrixGroup>> UpdateMatrixGroup(HotelSearchRequest request)
    {
        var matrixGroups = await CreateMatrixGroup();
        HotelSearchWithSpecification specification;
        var matrixItems = matrixGroups.First(_ => _.FilterKeyGroup == FilterKey.HotelAreaId);
        var requestClone = request.DeepClone();

        // HotelAreaId
        requestClone.FilterRequest.IdsFilters.RemoveIf(_ => _.FilterKey == FilterKey.HotelAreaId);
        specification = new HotelSearchWithSpecification(requestClone);
        matrixItems.MatrixItems = await CreateMatrixItems(specification, matrixItems.FilterKeyGroup, FilterRequestType.IDs);

        // AccommodationType
        requestClone = request.DeepClone();
        requestClone.FilterRequest.IdsFilters.RemoveIf(_ => _.FilterKey == FilterKey.AccommodationType);
        specification = new HotelSearchWithSpecification(requestClone);
        matrixItems = matrixGroups.First(_ => _.FilterKeyGroup == FilterKey.AccommodationType);
        matrixItems.MatrixItems = await CreateMatrixItems(specification, matrixItems.FilterKeyGroup, FilterRequestType.IDs);

        // Start rating
        requestClone = request.DeepClone();
        requestClone.FilterRequest.RangeFilters.RemoveIf(_ => _.FilterKey == FilterKey.StarRating);
        specification = new HotelSearchWithSpecification(requestClone);
        matrixItems = matrixGroups.First(_ => _.FilterKeyGroup == FilterKey.StarRating);
        matrixItems.MatrixItems = await CreateMatrixItems(specification, matrixItems.FilterKeyGroup, FilterRequestType.Range);

        return matrixGroups;
    }

    protected async Task<List<MatrixItem>> CreateMatrixItems(
        HotelSearchWithSpecification specification,
        FilterKey filterKey,
        FilterRequestType filterRequestType)
    {
        var hotels = await _unitOfWork.Repository<Hotel>()
              .FindAsync(
                  expression: specification.Criteria,
                  includeFunc: _ => _.Include(_ => _.Address.Area)
                                     .Include(_ => _.Category));

        var groupFilter = filterKey switch
        {
            // lưu ý: nếu GroupBy là class thì class đó phải có Equals GetHashCode
            // nếu dùng new GroupFilter phải có Equals GetHashCode trong GroupFilter
            // nếu dùng _.Category phải có Equals GetHashCode trong GroupFilter Category
            // nếu không muốn Equals GetHashCode thì dùng anonymous funtion new { } , lưu ý tất cả các case phải có param anonymous funtion new { } giống nhau
            FilterKey.HotelAreaId => hotels.GroupBy(_ => new
            {
                _.Address.Area.Id,
                _.Address.Area.Name,
            }),
            FilterKey.AccommodationType => hotels.GroupBy(_ => new
            {
                _.Category.Id,
                _.Category.Name,
            }),
            FilterKey.StarRating => hotels.GroupBy(_ => new
            {
                // do không có properties name Id , Name nên phải map
                Id = _.ReviewRating,
                Name = $"{_.ReviewRating} rating",
            }),
            _ => throw new BadRequestException($"`{filterKey}` not supporter for response filter")
        };

        return groupFilter.Select(_ => new MatrixItem
        {
            Id = _.Key.Id,
            Name = _.Key.Name,
            Count = _.Count(),
            FilterKey = filterKey,
            FilterRequestType = filterRequestType,
        }).OrderByDescending(_ => _.Count).ThenBy(_ => _.Id).ToList();
    }

    protected Task<List<MatrixGroup>> CreateMatrixGroup()
    {
        var matrixGroups = new List<MatrixGroup>
        {
            new MatrixGroup
            {
                FilterKeyGroup = FilterKey.AccommodationType,
                MatrixItems = new List<MatrixItem>()
            },
            new MatrixGroup
            {
                FilterKeyGroup = FilterKey.HotelAreaId,
                MatrixItems = new List<MatrixItem>()
            },
            new MatrixGroup
            {
                FilterKeyGroup = FilterKey.StarRating,
                MatrixItems = new List<MatrixItem>()
            },
            new MatrixGroup
            {
                FilterKeyGroup = FilterKey.Price,
                MatrixItems = new List<MatrixItem>
                {
                    new()
                    {
                        Id = 0,
                        Name = "Price",
                        Count = 0,
                        FilterKey = FilterKey.Price,
                        FilterRequestType = FilterRequestType.Range,
                    }
                }
            },
            new MatrixGroup
            {
                FilterKeyGroup = FilterKey.Name,
                MatrixItems = new List<MatrixItem>
                {
                    new()
                    {
                        Id = 0,
                        Name = "Tên khách sạn",
                        Count = 0,
                        FilterKey = FilterKey.Name,
                        FilterRequestType = FilterRequestType.Text,
                    }
                }
            }
        };

        return Task.FromResult(matrixGroups);
    }

    protected Task<List<SortMatrix>> CreateSortMatrix()
    {
        var sortMatrix = new List<SortMatrix>
        {
            new SortMatrix
            {
                FieldId = SortField.Ranking,
                Sorting = new()
                {
                    SortField = SortField.Ranking,
                    SortOrder = SortOrder.Desc
                },
                Display = "Phù hợp nhất"
            },
            new SortMatrix
            {
                FieldId = SortField.Price,
                Sorting = new()
                {
                    SortField = SortField.Price,
                    SortOrder = SortOrder.Asc
                },
                Display = "Giá thấp nhất trước"
            },
            new SortMatrix
            {
                FieldId = SortField.ReviewScore,
                Sorting = new()
                {
                    SortField = SortField.ReviewScore,
                    SortOrder = SortOrder.Desc
                },
                Display = "ĐÁNH GIÁ"
            }
        };

        return Task.FromResult(sortMatrix);
    }

    public async Task<HotelDetailResponse> FindHotelAsync(RoomTypeSearchRequest request)
    {
        var checkInDate = request.SearchCriteria.CheckInDate;
        var checkOutDate = request.SearchCriteria.CheckOutDate;

        var hotelResponse = (await _unitOfWork.Repository<Hotel>()
            .FindByAsync<HotelDetailResponse>(_mapper.ConfigurationProvider, h => h.Id == request.HotelId))
          .OrElseThrow(() => new NotFoundException(nameof(Hotel), request.HotelId));

        hotelResponse.MasterRooms = await _unitOfWork.Repository<RoomType>()
            .FindAsync<RoomTypeResponse>(
               configuration: _mapper.ConfigurationProvider,
               expression: r => r.HotelId == hotelResponse.Id &&
                                r.ReservationDetails.Where(rd =>
                                    rd.Reservation.Status != ReservationStatus.Canceled &&
                                   (checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate))
                                .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability,
               orderBy: r => r.OrderBy(_ => _.Price));

        hotelResponse.SoldOutRooms = await _unitOfWork.Repository<RoomType>()
            .FindAsync<RoomTypeSoldOutResponse>(
               configuration: _mapper.ConfigurationProvider,
               expression: r => r.HotelId == hotelResponse.Id &&
                                r.ReservationDetails.Where(rd =>
                                    rd.Reservation.Status != ReservationStatus.Canceled &&
                                   (checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate))
                                .Sum(rd => rd.Quantity) + request.Quantity > r.Availability);

        return hotelResponse;
    }

    public async Task<PaginatedResponse<ReviewResponse>> GetReviewsAsync(int hotelId, int pageIndex, int pageSize)
    {
        if (!await _unitOfWork.Repository<Hotel>().ExistsByAsync(c => c.Id == hotelId))
            throw new NotFoundException(nameof(Hotel), hotelId);

        var paginatedReview = await _unitOfWork.Repository<Review>()
             .FindAsync<ReviewResponse>(
                 configuration: _mapper.ConfigurationProvider,
                 pageIndex: pageIndex,
                 pageSize: pageSize,
                 expression: r => r.ReservationDetail.RoomType.HotelId == hotelId &&
                                  r.ReservationDetail.Reservation.Status == ReservationStatus.Confirmed,
                 orderBy: r => r.OrderByDescending(_ => _.CreatedAt));

        return await paginatedReview.ToPaginatedResponseAsync();
    }
}
