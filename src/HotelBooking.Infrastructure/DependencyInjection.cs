using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.Data.Interceptors;
using HotelBooking.Infrastructure.Data.SeedData;
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
        services.AddRepositories();
        services.AddDbContext(configuration);
        services.AddInitialiseDatabase();
        services.AddDefaultIdentity();

    }

    private static void AddServices(this IServiceCollection services)
    {
        //services
        //    .AddScoped<TokenHelper<User>>()
        //    .AddScoped<IAuthService, AuthService>()
        //    .AddScoped<ICurrentUserService, CurrentUserService>()
        //    .AddTransient<IEmailSender, EmailSender>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        //services
        //.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        string defaultConnection = configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
           options.UseMySql(defaultConnection, ServerVersion.AutoDetect(defaultConnection),
               builder => options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()))
                  .UseLazyLoadingProxies()
                  .EnableSensitiveDataLogging()
                  .EnableDetailedErrors());

    }

    private static void AddDefaultIdentity(this IServiceCollection services)
    {

        //services.AddIdentity<User, IdentityRole<int>>(options =>
        //{
        //    options.Password.RequireDigit = false;
        //    options.Password.RequireLowercase = false;
        //    options.Password.RequireNonAlphanumeric = false;
        //    options.Password.RequireUppercase = false;
        //    options.Password.RequiredLength = 1;
        //    options.Password.RequiredUniqueChars = 0;
        //    options.User.RequireUniqueEmail = true;
        //    //options.Stores.ProtectPersonalData = true;
        //}).AddEntityFrameworkStores<ApplicationDbContext>()
        //  //.AddPersonalDataProtection<LookupProtector, KeyRing>()
        //  .AddDefaultTokenProviders();
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
            //await initialiser.MigrateAsync();
            //await initialiser.DeletedAndMigrateAsync();
            //await initialiser.SeedAsync();
        }

        if (app.Environment.IsProduction())
        {
            //await initialiser.MigrateAsync();
        }
    }
}
