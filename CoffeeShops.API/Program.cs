using CoffeeShops.API.Data;
using CoffeeShops.API.Models;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Build OData EDM model
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EnableLowerCamelCase();
odataBuilder.EntitySet<CoffeeShop>("CoffeeShops");
var edmModel = odataBuilder.GetEdmModel();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .AddRouteComponents("odata", edmModel));

// Configure DbContext with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CoffeeShopsDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add Memory Cache for 24-hour caching
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheService>();

// Shop photos via Google Places. Key from GoogleApi:ApiKey (env: GoogleApi__ApiKey).
builder.Services.AddHttpClient<GooglePlacePhotoService>(client => client.Timeout = TimeSpan.FromSeconds(10));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseODataRouteDebug();

app.MapControllers();

app.Run();
