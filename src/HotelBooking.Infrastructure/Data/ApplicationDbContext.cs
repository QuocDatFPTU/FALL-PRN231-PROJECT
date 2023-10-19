using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HotelBooking.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public virtual DbSet<Amenity> Amenities { get; set; }
    public virtual DbSet<Guest> Guests { get; set; }
    public virtual DbSet<Hotel> Hotels { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
    public virtual DbSet<ReservationDetail> ReservationDetails { get; set; }
    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
