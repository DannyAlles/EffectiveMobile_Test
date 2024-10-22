using Data;
using Data.Interfaces;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net;
using System.Reflection;

const string _corsPolicy = "EnableAll";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Delivery API",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _corsPolicy,
                    builder =>
                    {
                        builder
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                    });
});

builder.Services.Configure<DeliveryConfigurationOptions>(builder.Configuration);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(IPAddress.Parse("172.17.0.1"));
});

builder.Services.AddDbContext<DeliveryDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddAutoMapper(assemblies: AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(profileAssemblyMarkerTypes: typeof(Program));

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

ConfigureRepositories(builder.Services);
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

await ApplyMigrations(app).ConfigureAwait(false);

app.MapControllers();

app.Run();

static void ConfigureRepositories(IServiceCollection services)
{
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IDistrictRepository, DistrictRepository>();
    services.AddScoped<ILogbookRepository, LogbookRepository>();
}

static void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IOrderService, OrderService>();
    services.AddScoped<ILogbookService, LogbookService>();
}

static async Task ApplyMigrations(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();
    using var db = scope.ServiceProvider.GetService<DeliveryDbContext>();
    await db.Database.MigrateAsync().ConfigureAwait(false);
}