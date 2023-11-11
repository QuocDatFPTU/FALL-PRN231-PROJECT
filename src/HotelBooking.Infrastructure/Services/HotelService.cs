using AutoMapper;
using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.DTOs.Reviews;
using HotelBooking.Application.DTOs.RoomTypes;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
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

    public async Task<PaginatedResponse<HotelResponse>> GetHotelsAsync(HotelSearchRequest request)
    {
        var checkInDate = request.SearchCriteria.CheckInDate;
        var checkOutDate = request.SearchCriteria.CheckoutDate;
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
        //        expression: h =>
        //                    h.Address.CityId == request.CityId &&
        //                    h.RoomTypes.Any(
        //                        r => r.ReservationDetails.Where(rd =>
        //                               checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
        //                               checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
        //                               rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
        //                               rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
        //                             .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability));

        var paginatedHotel = await _unitOfWork.Repository<Hotel>()
            .FindAsync<HotelResponse>(
                _mapper.ConfigurationProvider,
                pageIndex,
                pageSize,
                h => h.Address.CityId == request.CityId);

        var roomTypes = await _unitOfWork.Repository<RoomType>()
            .FindAsync(
                r => paginatedHotel.Select(_ => _.Id).Contains(r.HotelId) &&
                r.ReservationDetails.Where(rd =>
                    checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
                .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability);

        paginatedHotel.ForEach(hotel =>
        {
            hotel.IsSoldOut = !roomTypes.Any(_ => _.HotelId == hotel.Id);
            hotel.PricePerNight = roomTypes.Where(_ => _.HotelId == hotel.Id).MinBy(_ => _.Price)?.Price;
        });
        return paginatedHotel.ToPaginatedResponse();
    }

    public async Task<HotelDetailResponse> FindHotelAsync(RoomTypeSearchRequest request)
    {
        var checkInDate = request.SearchCriteria.CheckInDate;
        var checkOutDate = request.SearchCriteria.CheckoutDate;

        var hotelResponse = (await _unitOfWork.Repository<Hotel>()
            .FindByAsync<HotelDetailResponse>(_mapper.ConfigurationProvider, h => h.Id == request.HotelId))
          .OrElseThrow(() => new NotFoundException(nameof(Hotel), request.HotelId));

        hotelResponse.MasterRooms = await _unitOfWork.Repository<RoomType>()
            .FindAsync<RoomTypeResponse>(
               configuration: _mapper.ConfigurationProvider,
               expression: r => r.HotelId == hotelResponse.Id &&
                                r.ReservationDetails.Where(rd =>
                                    checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
                                .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability,
               orderBy: r => r.OrderBy(_ => _.Price));

        hotelResponse.SoldOutRooms = await _unitOfWork.Repository<RoomType>()
            .FindAsync<RoomTypeSoldOutResponse>(
               configuration: _mapper.ConfigurationProvider,
               expression: r => r.HotelId == hotelResponse.Id &&
                                r.ReservationDetails.Where(rd =>
                                    checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
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
                 expression: r => r.ReservationDetail.RoomType.HotelId == hotelId,
                 orderBy: r => r.OrderByDescending(_ => _.CreatedAt));

        return paginatedReview.ToPaginatedResponse();
    }
}
