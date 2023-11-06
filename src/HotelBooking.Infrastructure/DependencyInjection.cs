using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Data.Interceptors;
using HotelBooking.Infrastructure.Data.SeedData;
using HotelBooking.Infrastructure.Repositories;
using HotelBooking.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotelBooking.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddServices();
        services.AddDbContext(configuration);
        services.AddRepositories();
        services.AddInitialiseDatabase();

    }

    private static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IVnPayService, VnPayService>()
            .AddScoped<TokenHelper<Guest>>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<ICityService, CityService>()
            .AddScoped<IHotelService, HotelService>()
            .AddTransient<IEmailSender, EmailSender>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        string defaultConnection = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
           options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection),
               builder => builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                  .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
                  .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                  .EnableSensitiveDataLogging()
                  .EnableDetailedErrors());

    }

    private static void AddInitialiseDatabase(this IServiceCollection services)
    {
        services
            .AddScoped<ApplicationDbContextInitialiser>();
    }

    public static async Task UseInitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        if (app.Environment.IsDevelopment())
        {
            //await initialiser.DeletedDatabaseAsync();
            await initialiser.MigrateAsync();
            //await initialiser.SeedAsync();
        }

        if (app.Environment.IsProduction())
        {
            await initialiser.MigrateAsync();
        }
    }
}
