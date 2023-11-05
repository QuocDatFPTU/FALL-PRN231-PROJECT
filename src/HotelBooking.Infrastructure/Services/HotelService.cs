using AutoMapper;
using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        //var hotelIQ = await _unitOfWork.Repository<Hotel>()
        //            .FindToIQueryableAsync(
        //            expression: h =>
        //            h.Address.CityId == request.CityId &&
        //            h.RoomTypes.Any(
        //                r => r.ReservationDetails.Where(rd =>
        //                    checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
        //                    checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
        //                    rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
        //                    rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
        //                .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability));

        var hotelIQ = await _unitOfWork.Repository<Hotel>()
                    .FindToIQueryableAsync(h => h.Address.CityId == request.CityId);

        var paginatedHotel = await _mapper.ProjectTo<HotelResponse>(hotelIQ).PaginatedListAsync(pageIndex, pageSize);

        foreach (var hotel in paginatedHotel)
        {
            var roomTypes = await (await _unitOfWork.Repository<RoomType>()
                             .FindToIQueryableAsync(
                                 r => r.HotelId == hotel.Id &&
                                 r.ReservationDetails.Where(rd =>
                                     checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                                     checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                                     rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                                     rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate)
                                .Sum(rd => rd.Quantity) + request.Quantity <= r.Availability))
                             .ToListAsync();

            hotel.IsSoldOut = roomTypes.Count == 0;
            hotel.PricePerNight = roomTypes.MinBy(_ => _.Price)?.Price;
        }
        return paginatedHotel.ToPaginatedResponse();
    }

}
