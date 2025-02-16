using FluentValidation;
using FluentValidation.AspNetCore;
using HRAnalytics.API.Middleware;
using HRAnalytics.API.Validators;
using HRAnalytics.Application.Extensions;
using HRAnalytics.Application.Settings;
using HRAnalytics.Core.Constants;
using HRAnalytics.Core.Interfaces;
using HRAnalytics.Infrastructure.Extension;
using HRAnalytics.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/hranalytics-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// Add Service Configurations
ConfigureServices(builder);

var app = builder.Build();

// Configure Middleware Pipeline
ConfigureMiddleware(app);

// Application Startup
StartApplication(app);

void ConfigureServices(WebApplicationBuilder builder)
{
    // Basic Services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddResponseCaching();

    // SPA Services
    builder.Services.AddSpaStaticFiles(configuration =>
    {
        configuration.RootPath = "ClientApp/dist";
    });

    // API Versioning
    ConfigureApiVersioning(builder.Services);

    // Core Services
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // Authorization
    ConfigureAuthorization(builder.Services);

    // FluentValidation
    ConfigureValidation(builder.Services);

    // JWT Authentication
    ConfigureJwt(builder);

    // Swagger
    ConfigureSwagger(builder);

    // CORS
    ConfigureCors(builder.Services);
}

void ConfigureMiddleware(WebApplication app)
{
    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        ConfigureSwaggerUI(app);
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseSpaStaticFiles();
    app.UseResponseCaching();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp";

        if (app.Environment.IsDevelopment())
        {
            spa.UseProxyToSpaDevelopmentServer("http://localhost:5173");
        }
    });
}

// ... (diðer konfigürasyon metodlarý ayný kalacak)

void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
    });
}

void StartApplication(WebApplication app)
{
    try
    {
        Log.Information("Starting web host");
        app.Run();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Host terminated unexpectedly");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}