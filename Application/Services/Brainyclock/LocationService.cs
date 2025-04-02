using Core.Entities;
using Core.Entities.Brainyclock;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services.Brainyclock;
using Core.Modals;
using EFCore.BulkExtensions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Brainyclock
{
    public class LocationService : ILocationService
    {
        private readonly RestClientService _restClientService;
        private readonly ILogger<EmployeeService> _logger;
        private readonly ISiteRepository _siteRepository;
        private readonly MySqlDbContext _context;

        public LocationService(
            RestClientService restClientService,
            ILogger<EmployeeService> logger,
            ISiteRepository siteRepository,
            MySqlDbContext context
            )
        {
            _restClientService = restClientService;
            _logger = logger;
            _siteRepository = siteRepository;
            _context = context;
        }

        public async Task<GenericResponse<LocationModal>> AddLocationAsync(LocationModal locationInput)
        {
            try
            {
                _logger.LogInformation("Preparing to sync site with the external API.");
                var response = await _restClientService.PostAsync<Site>("addlocation", locationInput);

                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Successfully synced site with the external API.");
                    return new GenericResponse<LocationModal>(true, "Employees synced successfully.", locationInput);
                }
                else
                {
                    string errorMessage = "Failed to sync site with external API.";
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        try
                        {
                            var contentJson = JObject.Parse(response.Content);
                            errorMessage = contentJson["msg"]?.ToString() ?? errorMessage;
                        }
                        catch (Exception parseEx)
                        {
                            _logger.LogError(parseEx, "Failed to parse error message from API response.");
                        }
                    }

                    _logger.LogError("Error from external API: {ErrorMessage}", errorMessage);
                    return new GenericResponse<LocationModal>(false, errorMessage, null);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while syncing site with the external API.");
                return new GenericResponse<LocationModal>(false, "An error occurred while syncing site.", null);
            }
        }
        public async Task<GenericResponse<List<Site>>> SyncSiteAndLocationAsync()
        {
            using var transaction = await _siteRepository.BeginTransactionAsync(); // Start a transaction

            try
            {
                _logger.LogInformation("Preparing to sync site with the external API.");
                var sites = await _siteRepository.GetAllSitesAsync();

                var locations = new List<Location>();
                foreach (var site in sites)
                {
                    var location = new Location()
                    {
                         CompanyId = 414,
                         LocationName = site.SITE_NAM,
                         Address = site.ADDR1 + " " + site.ADDR2 + " " + site.ADDR3,
                         City = site.ADDR2,
                         State = site.ADDR3,
                         Country = "",
                         Pincode = "",
                         GeofenceRadius = 500,
                         Latitude = site.Latitude.ToString(),
                         Longitude = site.Longitude.ToString(),
                         SiteId = site.SiteID
                    };

                    locations.Add(location);
                }

                await _context.AddRangeAsync(locations);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new GenericResponse<List<Site>>(true, "Locations synced successfully.", sites);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback transaction on exception
                _logger.LogError(ex, "An error occurred while syncing site with the external API.");
                return new GenericResponse<List<Site>>(false, "An error occurred while syncing site.", null);
            }
        }
    }
}
