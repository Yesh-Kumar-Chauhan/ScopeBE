using Core.DTOs.Report;
using Core.Entities;
using Core.Modals;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Reports
{
    public partial class ReportService
    {
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteInformationAsync(GenerateReportDto reportDto)
        {
            try
            {
                // Call the repository to execute the stored procedure and fetch the data
                var siteInformationData = await _reportRepository.GetSiteInformationReportDataAsync(
                    reportDto.SiteId,
                    reportDto.DistrictId,
                    reportDto.Selections, // Assuming 'Selections' is equivalent to 'SiteType'
                    reportDto.Type
                );

                // Check if the data is empty
                if (siteInformationData == null || !siteInformationData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Convert the data to JSON or another suitable format for the client
                //var jsonData = System.Text.Json.JsonSerializer.Serialize(siteInformationData);

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteInformationData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteAssignmentDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var siteAssignmentData = await _reportRepository.ExecuteStoredProcedureAsync("sp_SitesAssignments_Report", parameters);

                // Check if the data is empty
                if (siteAssignmentData == null || !siteAssignmentData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteAssignmentData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSitePhoneDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var sitePhoneData = await _reportRepository.ExecuteStoredProcedureAsync("sp_SitePhone_Report", parameters);

                // Check if the data is empty
                if (sitePhoneData == null || !sitePhoneData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", sitePhoneData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateFirstAidExpirationDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int32);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var firstAidData = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report003_FirstAidExpiration_Report", parameters);

                // Check if the data is empty
                if (firstAidData == null || !firstAidData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", firstAidData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateMATExpirationBySiteReportDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var matExpirationData = await _reportRepository.ExecuteStoredProcedureAsync("sp_MATExpiration_Report", parameters);

                // Check if the data is empty
                if (matExpirationData == null || !matExpirationData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", matExpirationData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateFingerprintWaiverAdditionalSitesAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@StartDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.DistrictId, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var fingerprintWaiverAdditionalSites = await _reportRepository.ExecuteStoredProcedureAsync("sp_FingerprintWaiverAdditionalSites_Report", parameters);

                // Check if the data is empty
                if (fingerprintWaiverAdditionalSites == null || !fingerprintWaiverAdditionalSites.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", fingerprintWaiverAdditionalSites },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateWaiverExpirationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                //parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var waiverExpiration = await _reportRepository.ExecuteStoredProcedureAsync("sp_WaiverExpiration_Report", parameters);

                // Check if the data is empty
                if (waiverExpiration == null || !waiverExpiration.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", waiverExpiration },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateFirstAidExpirationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var firstAidData = await _reportRepository.ExecuteStoredProcedureAsync("sp_FirstAidExpiration_Report", parameters);

                // Check if the data is empty
                if (firstAidData == null || !firstAidData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", firstAidData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSitePermitExpirationReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var firstAidData = await _reportRepository.ExecuteStoredProcedureAsync("sp_SitePermitExpiration_Report", parameters);

                // Check if the data is empty
                if (firstAidData == null || !firstAidData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", firstAidData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateEmergencyPhoneAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@StartDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.DistrictId, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var emergencyPhoneData = await _reportRepository.ExecuteStoredProcedureAsync("sp_EmergencyPhone_Report", parameters);

                // Check if the data is empty
                if (emergencyPhoneData == null || !emergencyPhoneData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", emergencyPhoneData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteAddressListAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@StartDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.DistrictId, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var siteAddressListData = await _reportRepository.ExecuteStoredProcedureAsync("sp_StaffAddressList_Report", parameters);

                // Check if the data is empty
                if (siteAddressListData == null || !siteAddressListData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteAddressListData },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteEmergencyInformationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@StartDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.DistrictId, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var siteEmergencyInformation = await _reportRepository.ExecuteStoredProcedureAsync("sp_EmergencySiteInfromation_Report", parameters);

                // Check if the data is empty
                if (siteEmergencyInformation == null || !siteEmergencyInformation.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteEmergencyInformation },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteSpaceAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@StartDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                //parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var sitesSpaceReport = await _reportRepository.ExecuteStoredProcedureAsync("sp_SitesSpace_Report", parameters);

                // Check if the data is empty
                if (sitesSpaceReport == null || !sitesSpaceReport.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", sitesSpaceReport },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteNurseDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@StartDate", reportDto.DistrictId, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.DistrictId, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var sitesSpaceReport = await _reportRepository.ExecuteStoredProcedureAsync("sp_SitesNurseVisit_Report", parameters);

                // Check if the data is empty
                if (sitesSpaceReport == null || !sitesSpaceReport.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", sitesSpaceReport },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateChildAbuseExpirationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var sitesSpaceReport = await _reportRepository.ExecuteStoredProcedureAsync("sp_ChildAbuseExpiration_Report", parameters);

                // Check if the data is empty
                if (sitesSpaceReport == null || !sitesSpaceReport.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", sitesSpaceReport },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteLicensorsAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var sitesSpaceReport = await _reportRepository.ExecuteStoredProcedureAsync("sp_SiteLicensors_Report", parameters);

                // Check if the data is empty
                if (sitesSpaceReport == null || !sitesSpaceReport.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", sitesSpaceReport },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSiteListforHCPAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int32);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var siteListforHCP = await _reportRepository.ExecuteStoredProcedureAsync("sp_SiteListforHCP_Report", parameters);

                // Check if the data is empty
                if (siteListforHCP == null || !siteListforHCP.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteListforHCP },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSitePermitNumberReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                //parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int32);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var siteListforHCP = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report092_SitePermitNumber_Report", parameters);

                // Check if the data is empty
                if (siteListforHCP == null || !siteListforHCP.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteListforHCP },         // Data populated with JSON string
                    { "FileContent", null },      // No file content
                    { "FileName", null }          // No file name
                };

                return new GenericResponse<Dictionary<string, object>>(true, "Report data retrieved successfully.", responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the site information report.");
                return new GenericResponse<Dictionary<string, object>>(false, ex.Message, null);
            }
        }

    }
}
