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
    app.UseResponseCaching();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}

void ConfigureApiVersioning(IServiceCollection services)
{
    services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    services.AddVersionedApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });
}

void ConfigureAuthorization(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Roles.Admin));
        options.AddPolicy("RequireManagerRole", policy => policy.RequireRole(Roles.Admin, Roles.Manager));
        options.AddPolicy("AllEmployees", policy => policy.RequireRole(Roles.Admin, Roles.Manager, Roles.Employee));
    });
}

void ConfigureValidation(IServiceCollection services)
{
    services.AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters()
        .AddValidatorsFromAssemblyContaining<AuthLoginRequestValidator>();
}

void ConfigureJwt(WebApplicationBuilder builder)
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });
}

void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(c =>
    {
        var provider = builder.Services.BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"HR Analytics API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = $"HR Analytics API {description.ApiVersion} versiyonu",
                Contact = new OpenApiContact
                {
                    Name = "HR Analytics Team",
                    Email = "support@hranalytics.com"
                },
                License = new OpenApiLicense
                {
                    Name = "HR Analytics License",
                    Url = new Uri("https://example.com/license")
                }
            });
        }

        ConfigureSwaggerAuth(c);
        ConfigureSwaggerXml(c);
    });
}

void ConfigureSwaggerAuth(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = @"JWT Authorization header using the Bearer scheme.
                     Enter 'Bearer' [space] and then your token in the text input below.
                     Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
}

void ConfigureSwaggerXml(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
}

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

void ConfigureSwaggerUI(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"HR Analytics API {description.GroupName}");
        }

        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
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