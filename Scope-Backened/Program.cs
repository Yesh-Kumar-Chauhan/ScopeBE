using DataMigrationLibrary.Services;
using Scope_Backened;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure logging
// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Log to console
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Day) // Log to file in JSON format with daily rolling
    .CreateLogger();

// Replace the default logging with Serilog
builder.Host.UseSerilog();

// Use extension methods for configuring services
builder.Services.AddDatabaseContexts(builder.Configuration)
                .AddCustomServices(builder.Configuration)
                .AddCustomAuthentication(builder.Configuration)
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomSwagger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Run the data migration for seeding
using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<DataMigrationService>();
    // use only for initial seeding data
    // await migrationService.MigrateDistrictsAsync();
    // await migrationService.MigrateSitesAsync();
    // await migrationService.MigratePersonelAsync();
    // await migrationService.MigrateReportsAsync();
    // await migrationService.MigrateSchoolsAsync();
    // await migrationService.MigrateWorkshopTypesAsync();
    // await migrationService.MigrateTopicTypesAsync();
    // await migrationService.MigrateInservicesAsync();
    // await migrationService.MigrateClosingsAsync();
    // await migrationService.MigrateStatusesAsync();
    // await migrationService.MigrateTimesheetsAsync();
    // await migrationService.MigrateVisitsAsync();
    // await migrationService.MigrateContactsAsync();
    // await migrationService.MigrateWorkshopsAsync();
    // await migrationService.MigrateAbsencesAsync();
    // await migrationService.MigrateAbsenceReasonsAsync();
    // await migrationService.MigrateAttendanceAsync();
    // await migrationService.MigrateWavierReceivedAsync();
    // await migrationService.MigrateCertificateTypeAsync();
    // await migrationService.MigrateCertificatesAsync();
    // await migrationService.MigrateWaiversSentAsync();
    // await migrationService.MigrateDirectorsAsync();

}

// Configure the HTTP request pipeline.
app.UseCustomRouting(app.Environment);
app.Run();
