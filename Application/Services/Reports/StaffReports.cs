using Core.DTOs.Report;
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffSalaryDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                // Call the repository to execute the stored procedure and fetch the data
                var staffSalaryData = await _reportRepository.GenerateStaffSalariesDataAsync(
                    reportDto.PersonelId,
                    reportDto.SiteId,
                    reportDto.DistrictId,
                    reportDto.CountryId,
                    reportDto.Selections,// Assuming 'Selections' is equivalent to 'SiteType'
                    reportDto.Type
                );

                // Check if the data is empty
                if (staffSalaryData == null || !staffSalaryData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffSalaryData },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffMatCprFaDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                // Call the repository to execute the stored procedure and fetch the data
                var staffMatCprFaData = await _reportRepository.GenerateStaffMatCprFaDataAsync(
                    reportDto.SiteId,
                    reportDto.DistrictId,
                    reportDto.StartDate,
                    reportDto.EndDate,
                    reportDto.CountryId,
                    reportDto.PersonelId,
                    reportDto.Selections,// Assuming 'Selections' is equivalent to 'SiteType'
                    reportDto.Type
                );

                // Check if the data is empty
                if (staffMatCprFaData == null || !staffMatCprFaData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffMatCprFaData },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffMaximumHourDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                // Call the repository to execute the stored procedure and fetch the data
                var staffMatCprFaData = await _reportRepository.GenerateStaffMaximumHourDataAsync(
                    reportDto.PersonelId,
                    reportDto.SiteId,
                    reportDto.DistrictId,
                    reportDto.CountryId,
                    reportDto.Selections,// Assuming 'Selections' is equivalent to 'SiteType'
                    reportDto.Type
                );

                // Check if the data is empty
                if (staffMatCprFaData == null || !staffMatCprFaData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffMatCprFaData },         // Data populated with JSON string
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

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffScheduleAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleData = await _reportRepository.ExecuteStoredProcedureAsync("sp_StaffSchedule_Report", parameters);

                // Check if the data is empty
                if (staffScheduleData == null || !staffScheduleData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleData },         // Data populated with JSON string
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

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffEmailAddressAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleData = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report022_StaffEmail_Report", parameters);

                // Check if the data is empty
                if (staffScheduleData == null || !staffScheduleData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleData },         // Data populated with JSON string
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

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffSignInGroupAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleData = await _reportRepository.ExecuteStoredProcedureAsync("sp_Stuff_SignIn_Report", parameters);

                // Check if the data is empty
                if (staffScheduleData == null || !staffScheduleData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleData },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffInserviceAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleData = await _reportRepository.ExecuteStoredProcedureAsync("sp_Inservices_Report", parameters);

                // Check if the data is empty
                if (staffScheduleData == null || !staffScheduleData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleData },         // Data populated with JSON string
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

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffTrackingAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleData = await _reportRepository.ExecuteStoredProcedureAsync("sp_StaffTracking_Report", parameters);

                // Check if the data is empty
                if (staffScheduleData == null || !staffScheduleData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleData },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateCPRExpirationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var cprExpiration = await _reportRepository.ExecuteStoredProcedureAsync("sp_CPRExpiration_Report", parameters);

                // Check if the data is empty
                if (cprExpiration == null || !cprExpiration.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", cprExpiration },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffSignInWithSiteAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var siteSignIn = await _reportRepository.ExecuteStoredProcedureAsync("sp_Stuff_SignIn_Report", parameters);

                // Check if the data is empty
                if (siteSignIn == null || !siteSignIn.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", siteSignIn },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateFirstAidChildAbuseAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var firstAidMatDateCPR = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report045_FirstAidMatDateCPR_Report", parameters);

                // Check if the data is empty
                if (firstAidMatDateCPR == null || !firstAidMatDateCPR.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", firstAidMatDateCPR },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffExpirationAlphaAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffExpirationAlpha = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report050_StaffExpirationAlpha", parameters);

                // Check if the data is empty
                if (staffExpirationAlpha == null || !staffExpirationAlpha.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffExpirationAlpha },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateFoundationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var foundation = await _reportRepository.ExecuteStoredProcedureAsync("sp_PersonnelFoundations_Report", parameters);

                // Check if the data is empty
                if (foundation == null || !foundation.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", foundation },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateEmptyFoundationAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                parameters.Add("@All", 1, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var foundation = await _reportRepository.ExecuteStoredProcedureAsync("sp_PersonnelFoundations_Report", parameters);

                // Check if the data is empty
                if (foundation == null || !foundation.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", foundation },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffDataOfEmploymentAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffDateOfEmployment = await _reportRepository.ExecuteStoredProcedureAsync("sp_StaffDateOfEmployment_Report", parameters);

                // Check if the data is empty
                if (staffDateOfEmployment == null || !staffDateOfEmployment.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffDateOfEmployment },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateExportStaffInfoReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffDateOfEmployment = await _reportRepository.ExecuteStoredProcedureAsync("sp_ExportStaffInfo_Report", parameters);

                // Check if the data is empty
                if (staffDateOfEmployment == null || !staffDateOfEmployment.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffDateOfEmployment },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffScheduleWithInfoAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleWithInfo = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report083_StaffSchedule", parameters);

                // Check if the data is empty
                if (staffScheduleWithInfo == null || !staffScheduleWithInfo.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleWithInfo },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateSexualHarassmentReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleWithInfo = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report086_SexualHarassment_Report", parameters);

                // Check if the data is empty
                if (staffScheduleWithInfo == null || !staffScheduleWithInfo.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleWithInfo },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStuffSignInWithoutSiteReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                //parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffScheduleWithInfo = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report087_Stuff_SignIn_WithoutSite_Report", parameters);

                // Check if the data is empty
                if (staffScheduleWithInfo == null || !staffScheduleWithInfo.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffScheduleWithInfo },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateCBCReportAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                //parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var cbc = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report088_CBC_Report", parameters);

                // Check if the data is empty
                if (cbc == null || !cbc.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", cbc },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffWorkshopsAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                //parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                //parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                //parameters.Add("@Type", reportDto.Type, DbType.Int64);
                //parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var worshop = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report090_StaffWorkshops_Report", parameters);

                // Check if the data is empty
                if (worshop == null || !worshop.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", worshop },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateInserviceStaffTotalsAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                //parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var inserviceStaffTotals = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report093_InserviceStaffTotals_Report", parameters);

                // Check if the data is empty
                if (inserviceStaffTotals == null || !inserviceStaffTotals.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", inserviceStaffTotals },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffChecklistsAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                //parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffCheckList = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report094_StaffChecklist_Report", parameters);

                // Check if the data is empty
                if (staffCheckList == null || !staffCheckList.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffCheckList },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffBirthMonthsAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", reportDto.PersonelId, DbType.Int64);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);
                parameters.Add("@Type", reportDto.Type, DbType.Int64);
                //parameters.Add("@All", 0, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var staffBirthMonth = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report095_StaffBirthMonth_Report", parameters);

                // Check if the data is empty
                if (staffBirthMonth == null || !staffBirthMonth.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffBirthMonth },         // Data populated with JSON string
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
