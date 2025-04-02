using Core.DTOs.Report;
using Core.Entities;
using Core.Modals;
using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateVisitsDataAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", reportDto.StartDate, DbType.DateTime);
                parameters.Add("@EndDate", reportDto.EndDate, DbType.DateTime);
                parameters.Add("@SiteID", reportDto.SiteId, DbType.Int64);
                parameters.Add("@DistrictID", reportDto.DistrictId, DbType.Int64);
                parameters.Add("@CountyID", reportDto.CountryId, DbType.String);
                parameters.Add("@SiteType", reportDto.Selections, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var visitsReportData = await _reportRepository.ExecuteStoredProcedureAsync("sp_Visits_Report", parameters);

                // Check if the data is empty
                if (visitsReportData == null || !visitsReportData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", visitsReportData },         // Data populated with JSON string
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
