using AutoMapper;
using Core.DTOs.Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Microsoft.Data.SqlClient;
using Azure;
using Core.DTOs.Site;
using Core.DTOs.Sites;
using Core.Interfaces.Services.Brainyclock;
using Application.Helpers;

namespace Application.Services
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SiteService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILocationService _locationService;
        private readonly GoogleMapsGeocodingServiceHelper _geocodingServiceHelper;

        public SiteService(ISiteRepository siteRepository, IMapper mapper,
            ILogger<SiteService> logger, IConfiguration configuration,
            GoogleMapsGeocodingServiceHelper geocodingServiceHelper,
            ILocationService locationService)
        {
            _siteRepository = siteRepository;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _geocodingServiceHelper = geocodingServiceHelper;
            _locationService = locationService;
        }

        public async Task<GenericResponse<IEnumerable<SiteDto>>> GetSitesWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _siteRepository.GetAllSites();

                // Apply filtering based on the search query
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(s => s.SITE_NUM.ToString().Contains(search) || s.SITE_NAM.Contains(search));
                }

                // Get the total count of items (before pagination)
                var totalItems = await query.CountAsync();

                // Apply pagination
                var sites = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var siteDtos = _mapper.Map<IEnumerable<SiteDto>>(sites);
                return new GenericResponse<IEnumerable<SiteDto>>(true, "Sites retrieved successfully.", siteDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all sites.");
                return new GenericResponse<IEnumerable<SiteDto>>(false, "An error occurred while retrieving sites.", null);
            }
        }

        public async Task<GenericResponse<List<SiteDto>>> GetAllSitesAsync()
        {
            try
            {
                var sites = await _siteRepository.GetAllSitesAsync();

                var siteDtos = _mapper.Map<List<SiteDto>>(sites);
                return new GenericResponse<List<SiteDto>>(true, "All sites retrieved successfully.", siteDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all sites.");
                return new GenericResponse<List<SiteDto>>(false, "An error occurred while retrieving sites.", null);
            }
        }

        public async Task<GenericResponse<SiteDto>> GetSiteByIdAsync(long id)
        {
            try
            {
                var site = await _siteRepository.GetSiteByIdAsync(id);
                if (site == null)
                {
                    _logger.LogWarning("Site with ID {SiteId} not found.", id);
                    return new GenericResponse<SiteDto>(false, "Site not found.", null);
                }

                var siteDto = _mapper.Map<SiteDto>(site);
                return new GenericResponse<SiteDto>(true, "Site retrieved successfully.", siteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving site with ID {SiteId}.", id);
                return new GenericResponse<SiteDto>(false, "An error occurred while retrieving the site.", null);
            }
        }

        public async Task<GenericResponse<SiteDto>> CreateSiteAsync(SiteDto siteDto)
        {
            using var transaction = await _siteRepository.BeginTransactionAsync();
            try
            {
                var site = _mapper.Map<Site>(siteDto);
                var addressParts = new List<string> { site.ADDR1, site.ADDR2, site.ADDR3 }
                                       .Where(part => !string.IsNullOrWhiteSpace(part))
                                       .ToList();

                var fullAddress = string.Join(", ", addressParts);
                var (latitude, longitude) = await _geocodingServiceHelper.GetLatLongFromAddressAsync(fullAddress);

                site.Latitude = latitude;
                site.Longitude = longitude;

                var createdSite = await _siteRepository.AddSiteAsync(site);
                var createdSiteDto = _mapper.Map<SiteDto>(createdSite);

                
                var location = new LocationModal()
                {
                    company_id = 414,
                    location_name = createdSite.SITE_NAM,
                    address = createdSite.ADDR1 + " " + createdSite.ADDR2 + " " + createdSite.ADDR3,
                    city = createdSite.ADDR2,
                    state = createdSite.ADDR3,
                    country = "",
                    pincode = "",
                    geofence_radius = 500,
                    latitude = createdSite.Latitude.ToString(),
                    longitude = createdSite.Longitude.ToString(),
                    scope_site_id = createdSite.SiteID
                };
                // Call the service to add the location
                var locationResponse = await _locationService.AddLocationAsync(location);

                // Check if the location was added successfully
                if (!locationResponse.Success)
                {
                    throw new Exception($"Failed to create location: {locationResponse.Message}");
                }

                // Commit the transaction if both site and location are added successfully
                await transaction.CommitAsync();
                return new GenericResponse<SiteDto>(true, "Site created successfully.", createdSiteDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while creating a new site.");
                return new GenericResponse<SiteDto>(false, "An error occurred while creating the site.", null);
            }
        }

        public async Task<GenericResponse<SiteDto>> UpdateSiteAsync(long id, SiteDto siteDto)
        {
            try
            {
                var site = _mapper.Map<Site>(siteDto);
                site.SiteID = id;

                var updatedSite = await _siteRepository.UpdateSiteAsync(site);
                if (updatedSite == null)
                {
                    _logger.LogWarning("Site with ID {SiteId} not found or update failed.", id);
                    return new GenericResponse<SiteDto>(false, "Site not found or update failed.", null);
                }

                var updatedSiteDto = _mapper.Map<SiteDto>(updatedSite);
                return new GenericResponse<SiteDto>(true, "Site updated successfully.", updatedSiteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating site with ID {SiteId}.", id);
                return new GenericResponse<SiteDto>(false, "An error occurred while updating the site.", null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteSiteAsync(long id)
        {
            try
            {
                var deleted = await _siteRepository.DeleteSiteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Site with ID {SiteId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Site not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Site deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting site with ID {SiteId}.", id);
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<GenericResponse<Dictionary<string, List<SiteByType>>>> GetSitesByTypesAsync(List<int> types)
        {
            try
            {

                var siteResults = new Dictionary<string, List<SiteByType>>();

                foreach (var type in types)
                {
                    var sites = await _siteRepository.GetSitesByTypeAsync(null, type);

                    switch (type)
                    {
                        case 4:
                            siteResults["beforeSites"] = sites;
                            break;
                        case 5:
                            siteResults["duringSites"] = sites;
                            break;
                        case 6:
                            siteResults["afterSites"] = sites;
                            break;
                        default:
                            // Optionally handle unexpected types
                            _logger.LogWarning($"Unexpected type: {type}");
                            break;
                    }
                }

                return new GenericResponse<Dictionary<string, List<SiteByType>>>(
                    true, "Sites retrieved successfully.", siteResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving sites by type.");
                return new GenericResponse<Dictionary<string, List<SiteByType>>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<ExtendedSiteDto>>> GetSitesByDistrictIdAsync(long districtId, long? districtNum = null, int operation = 0)
        {
            try
            {
                var dtSites = await _siteRepository.GetSitesByDistrictIdAsync(districtId, districtNum, operation);

                return new GenericResponse<List<ExtendedSiteDto>>(true, "Sites retrieved successfully.", dtSites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new GenericResponse<List<ExtendedSiteDto>>(false, ex.Message, null);
            }
        }
        
        public async Task<GenericResponse<List<SiteDto>>> GetSitesByOperationAsync(string keyword, int operation = 0)
        {
            try
            {
                var dtSites = await _siteRepository.GetSitesByOperationAsync(keyword, operation);

                var sites = _mapper.Map<List<SiteDto>>(dtSites);
                return new GenericResponse<List<SiteDto>>(true, "Sites retrieved successfully.", sites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new GenericResponse<List<SiteDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<SiteNamesDto>>> GetSitesByIdsAsync(List<long?> ids)
        {
            try
            {
                var sites = await _siteRepository.GetAllSitesByIdAsync(ids);

                var sitesDto = _mapper.Map<List<SiteNamesDto>>(sites);
                return new GenericResponse<List<SiteNamesDto>>(true, "Sites retrieved successfully.", sitesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new GenericResponse<List<SiteNamesDto>>(false, ex.Message, null);
            }
        }
    }
}
