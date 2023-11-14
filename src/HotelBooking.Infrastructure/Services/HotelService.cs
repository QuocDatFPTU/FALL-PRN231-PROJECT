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
using System.Linq.Expressions;

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
            MatrixGroupFilters = await CreateMatrixGroup(request)
        };
    }

    protected async Task<List<MatrixGroup>> CreateMatrixGroup(HotelSearchRequest request)
    {
        var matrixGroups = new List<MatrixGroup>
        {
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

        Expression<Func<Hotel, bool>> expressionHotelArea = _ => _.Address.CityId == request.CityId;
        Expression<Func<Hotel, bool>> expressionHotelCategory = _ => _.Address.CityId == request.CityId;
        Expression<Func<Hotel, bool>> expressionHotelStarRating = _ => _.Address.CityId == request.CityId;

        var filterRequest = request.FilterRequest;

        if (filterRequest != null)
        {
            var idsFilters = filterRequest.IdsFilters;
            var rangeFilters = filterRequest.RangeFilters;
            var textFilters = filterRequest.TextFilters;

            if (!idsFilters.IsNullOrEmpty())
            {
                idsFilters = idsFilters!.DistinctBy(_ => _.FilterKey).ToList();
                foreach (var item in idsFilters!)
                {
                    switch (item.FilterKey)
                    {
                        case FilterKey.HotelAreaId:
                            expressionHotelCategory = expressionHotelCategory.AndAlso(_ => item.Ids.Contains(_.Address.AreaId));
                            expressionHotelStarRating = expressionHotelStarRating.AndAlso(_ => item.Ids.Contains(_.Address.AreaId));
                            break;
                        case FilterKey.AccommodationType:
                            expressionHotelArea = expressionHotelArea.AndAlso(_ => item.Ids.Contains(_.CategoryId));
                            expressionHotelStarRating = expressionHotelStarRating.AndAlso(_ => item.Ids.Contains(_.CategoryId));
                            break;
                        default:
                            throw new BadRequestException($"`{item.FilterKey}` not supporter for idsFilters");
                    }
                }
            }

            if (!rangeFilters.IsNullOrEmpty())
            {
                rangeFilters = rangeFilters!.DistinctBy(_ => _.FilterKey).ToList();
                foreach (var item in rangeFilters!)
                {
                    Expression<Func<Hotel, bool>> expression = _ => false;
                    switch (item.FilterKey)
                    {
                        case FilterKey.Price:

                            foreach (var range in item.Ranges)
                            {
                                expression = expression.OrElse(_ => _.RoomTypes.Any(_ => _.Price >= range.From && _.Price <= range.To));
                            }
                            expressionHotelStarRating = expressionHotelStarRating.AndAlso(expression);
                            expressionHotelArea = expressionHotelArea.AndAlso(expression);
                            expressionHotelCategory = expressionHotelCategory.AndAlso(expression);
                            break;
                        case FilterKey.StarRating:

                            foreach (var range in item.Ranges)
                            {
                                expression = expression.OrElse(_ => _.ReviewRating >= range.From && _.ReviewRating < range.To);
                            }
                            expressionHotelArea = expressionHotelArea.AndAlso(expression);
                            expressionHotelCategory = expressionHotelCategory.AndAlso(expression);
                            break;
                        default:
                            throw new BadRequestException($"`{item.FilterKey}` not supporter for rangeFilters");
                    }
                }
            }

            if (!textFilters.IsNullOrEmpty())
            {
                textFilters = textFilters!.DistinctBy(_ => _.FilterKey).ToList();
                foreach (var item in textFilters!)
                {
                    switch (item.FilterKey)
                    {
                        case FilterKey.Name:
                            expressionHotelArea = expressionHotelArea.AndAlso(_ => EF.Functions.Like(_.Name, $"%{item.Text}%"));
                            expressionHotelCategory = expressionHotelCategory.AndAlso(_ => EF.Functions.Like(_.Name, $"%{item.Text}%"));
                            expressionHotelStarRating = expressionHotelStarRating.AndAlso(_ => EF.Functions.Like(_.Name, $"%{item.Text}%"));
                            break;
                        default:
                            throw new BadRequestException($"`{item.FilterKey}` not supporter for textFilters");
                    }
                }
            }
        }

        var matrixGroup = new MatrixGroup
        {
            FilterKeyGroup = FilterKey.HotelAreaId,
            MatrixItems = new List<MatrixItem>()
        };

        (await _unitOfWork.Repository<Hotel>().FindAsync(expression: expressionHotelArea, includeFunc: _ => _.Include(_ => _.Address.Area)))
        .GroupBy(hotel => new
        {
            hotel.Address.AreaId,
            hotel.Address.Area.Name
        }).Select(group => new
        {
            Area = group.Key,
            HotelCount = group.Count()
        }).ToList().ForEach(item =>
        {
            var matrixItem = new MatrixItem()
            {
                Id = item.Area.AreaId,
                Name = item.Area.Name,
                Count = item.HotelCount,
                FilterKey = FilterKey.HotelAreaId,
                FilterRequestType = FilterRequestType.IDs,
            };

            matrixGroup.MatrixItems.Add(matrixItem);

        });
        matrixGroups.Add(matrixGroup);

        matrixGroup = new MatrixGroup
        {
            FilterKeyGroup = FilterKey.AccommodationType,
            MatrixItems = new List<MatrixItem>()
        };

        (await _unitOfWork.Repository<Hotel>().FindAsync(expression: expressionHotelCategory, includeFunc: _ => _.Include(_ => _.Category)))
       .GroupBy(hotel => new
       {
           hotel.Category.Id,
           hotel.Category.Name
       }).Select(group => new
       {
           Category = group.Key,
           HotelCount = group.Count()
       }).ToList().ForEach(item =>
       {
           var matrixItem = new MatrixItem()
           {
               Id = item.Category.Id,
               Name = item.Category.Name,
               Count = item.HotelCount,
               FilterKey = FilterKey.AccommodationType,
               FilterRequestType = FilterRequestType.IDs,
           };

           matrixGroup.MatrixItems.Add(matrixItem);

       });
        matrixGroups.Add(matrixGroup);

        matrixGroup = new MatrixGroup
        {
            FilterKeyGroup = FilterKey.StarRating,
            MatrixItems = new List<MatrixItem>()
        };

        (await _unitOfWork.Repository<Hotel>().FindAsync(expression: expressionHotelStarRating))
       .GroupBy(hotel => new
       {
           hotel.ReviewRating
       }).Select(group => new
       {
           Rating = group.Key,
           HotelCount = group.Count()
       }).ToList().ForEach(item =>
       {
           var matrixItem = new MatrixItem()
           {
               Id = item.Rating.ReviewRating,
               Name = "",
               Count = item.HotelCount,
               FilterKey = FilterKey.StarRating,
               FilterRequestType = FilterRequestType.Range,
           };

           matrixGroup.MatrixItems.Add(matrixItem);

       });
        matrixGroups.Add(matrixGroup);

        foreach (var item in matrixGroups)
        {
            item.MatrixItems = item.MatrixItems.OrderByDescending(_ => _.Count).ToList();
        }

        return matrixGroups;

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
