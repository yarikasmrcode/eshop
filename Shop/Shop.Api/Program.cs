using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shop.Api.Data;
using Shop.Api.Errors;
using Shop.Api.Extensions;
using Shop.Api.Helpers;
using Shop.Api.Middleware;
using Shop.Core.Entities.Identity;
using Shop.Core.Interfaces;
using Shop.Infrastructure.Data.Repositories;
using Shop.Infrastructure.Identity;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securitySchema = new OpenApiSecurityScheme()
    {
        Description = "Jwt auth Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirments = new OpenApiSecurityRequirement()
    {
        {securitySchema, new []{"Bearer"}}
    };

    c.AddSecurityRequirement(securityRequirments);
});

builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddDbContext<StoreContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder
        .Configuration
        .GetConnectionString("Redis"), true);
    ;
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    var context = services.GetRequiredService<StoreContext>();
    //await StoreContextSeed.SeedAsync(context, loggerFactory

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();

    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, ex.Message);
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


await app.RunAsync();
