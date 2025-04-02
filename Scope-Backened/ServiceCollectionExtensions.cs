using Application.Helpers;
using Application.Services;
using Application.Services.Brainyclock;
using Application.Services.Inservice;
using Application.Services.Personal;
using Application.Services.Reports;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Interfaces.Services.Brainyclock;
using Core.Mappings;
using DataMigrationLibrary.Data;
using DataMigrationLibrary.Services;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Scope_Backened
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure the old database connection
            services.AddDbContext<LegacyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DevConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                })); ;

            // Configure the new database connection
            services.AddDbContext<NewAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ProdConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }));
            
            // Configure the main application database connection
            string connectionString = configuration.GetConnectionString("ProdConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));


            // Configure the MySQL database connection
            services.AddDbContext<MySqlDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                    new MySqlServerVersion(new Version(8, 0, 23)), // Specify your MySQL version here
                    mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    }));

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Add the data migration service
            services.AddTransient<DataMigrationService>();

            // Register custom services
            services.AddScoped<TokenValidationService>();
            services.AddScoped<RestClientService>();
            services.AddScoped<GoogleMapsGeocodingServiceHelper>(); // Register GoogleMapsGeocodingServiceHelper
            services.Configure<GoogleMapsSettings>(configuration.GetSection("GoogleMaps")); // Configure Google Maps API key


            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IPersonelService, PersonelService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IInserviceService, InserviceService>();
            services.AddScoped<IInserviceDropdownService, InserviceDropdownService>();
            services.AddScoped<IClosingService, ClosingService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IWorkshopService, WorkshopService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISchedularTimesheetService, SchedularTimesheetService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IWaiversSentService, WaiversSentService>();
            services.AddScoped<IDirectorService, DirectorService>();

            //Brainyclock
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILocationService, LocationService>();

            // Register custom Repositories
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IPersonelRepository, PersonelRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<IInserviceRepository, InserviceRepository>();
            services.AddScoped<IWorkshopTypeRepository, WorkshopTypeRepository>();
            services.AddScoped<ITopicTypeRepository, TopicTypeRepository>();
            services.AddScoped<IClosingRepository, ClosingRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITimesheetRepository, TimesheetRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IWorkshopRepository, WorkshopRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISchedularTimesheetRepository, SchedularTimesheetRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IWaiversSentRepository, WaiversSentRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var azureAdSettings = configuration.GetSection("AzureAd").Get<AzureAdSettings>();

            services.Configure<AzureAdSettings>(configuration.GetSection("AzureAd"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://login.microsoftonline.com/{azureAdSettings.TenantId}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://login.microsoftonline.com/{azureAdSettings.TenantId}/v2.0",
                        ValidateAudience = true,
                        ValidAudience = azureAdSettings.ClientId,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadAccess", policy => policy.RequireClaim("scp", "Read.All"));
            });

            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            //if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            return app;
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
        {
            app.UseCors("AllowAll");
            return app;
        }

        public static IApplicationBuilder UseCustomSecurity(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static IApplicationBuilder UseCustomRouting(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCustomSecurity();

            app.UseCustomCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // This maps attribute-routed controllers
            });

            app.UseCustomSwagger();

            return app;
        }

    }
}
