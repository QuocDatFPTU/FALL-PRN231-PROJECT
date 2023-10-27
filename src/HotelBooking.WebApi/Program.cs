using HotelBooking.Application;
using HotelBooking.Infrastructure;
using HotelBooking.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

await app.UseWebApplication();

app.Run();
