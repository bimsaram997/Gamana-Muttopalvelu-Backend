
using Gamana_Muttopalvelu_Backend.Controllers;
using Gamana_Muttopalvelu_Backend.Data;
using Gamana_Muttopalvelu_Backend.Options;
using Gamana_Muttopalvelu_Backend.Repositories;
using Gamana_Muttopalvelu_Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClientPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://gamana-muttopalvelu-frontend-dev.onrender.com", "https://gamana-muttopalvelu-frontend-prod-33g9.onrender.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10)); //  Tells Firefox to cache the approval for 10 minutes
    });
});

builder.Services.Configure<DigitransitOptions>(
    builder.Configuration.GetSection(DigitransitOptions.Position));
builder.Services.AddHttpClient<AddressController>();
// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IRequestedServiceRepository, RequestedServiceRepository>();



builder.Services.AddHttpClient<IRouteService, RouteService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "GamanaMuuttopalveluBackend/1.0 (contact@gamana.fi)");
});
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Register Email Service
builder.Services.AddScoped<IEmailService, EmailService>();
// Register the Email Queue as a Singleton
builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
// Register the Hosted Background Service
builder.Services.AddHostedService<EmailBackgroundWorker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gamana API v1");

    // Optional: This makes Swagger the default page at the root URL (/)
    // If you prefer typing /swagger locally, you can leave this line out!
    c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.UseRouting(); // 1. Set up routing metadata first

// ===================================================================
// FIX: ACTIVATED CORS MIDDLEWARE HERE WITH YOUR EXACT POLICY NAME
// ===================================================================
app.UseCors("AngularClientPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
