using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Infrastructure.Data.SeedData;

public class ApplicationDbContextInitialiser
{

    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task MigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task DeletedDatabaseAsync()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        //await _context.RoomTypes.ExecuteUpdateAsync(_ => _.SetProperty(_ => _.Price, _ => _.Price * 1000));
        //await _context.ReservationDetails.ExecuteUpdateAsync(_ => _.SetProperty(_ => _.Price, _ => _.Price * 1000));
        //await _context.Reservations.ExecuteUpdateAsync(_ => _.SetProperty(_ => _.Status, _ => ReservationStatus.Confirmed));
        //await _context.Reservations
        //    .Select(_ => new { Reservation = _, TotalQuantity = _.ReservationDetails.Sum(_ => _.Quantity) })
        //    .ExecuteUpdateAsync(_ => _.SetProperty(_ => _.Reservation.TotalQuantity, _ => _.TotalQuantity));
        //await _context.Reservations
        //    .Select(_ => new { Reservation = _, TotalAmount = _.ReservationDetails.Sum(_ => _.Price) })
        //    .ExecuteUpdateAsync(_ => _.SetProperty(_ => _.Reservation.TotalAmount, _ => _.TotalAmount));

    }
}
