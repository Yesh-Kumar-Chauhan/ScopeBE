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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateWorkshopAsync(GenerateReportDto reportDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@WorkshopID", reportDto.SiteId, DbType.Int64);

                // Call the repository to execute the stored procedure and fetch the data
                var workshopReportData = await _reportRepository.ExecuteStoredProcedureAsync("sp_Report089_Workshops_Report", parameters);

                // Check if the data is empty
                if (workshopReportData == null || !workshopReportData.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", workshopReportData },         // Data populated with JSON string
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
