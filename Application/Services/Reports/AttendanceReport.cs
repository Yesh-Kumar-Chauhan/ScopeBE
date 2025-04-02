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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateTotalAttendanceReportAsync(GenerateReportDto reportDto)
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
                var totalAttendance = await _reportRepository.ExecuteStoredProcedureAsync("sp_AttendancesSummery_Report", parameters);

                // Check if the data is empty
                if (totalAttendance == null || !totalAttendance.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", totalAttendance },         // Data populated with JSON string
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

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffAttendanceGroupAsync(GenerateReportDto reportDto)
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
                var staffAttendanceGroup = await _reportRepository.ExecuteStoredProcedureAsync("sp_Attendances_Report", parameters);

                // Check if the data is empty
                if (staffAttendanceGroup == null || !staffAttendanceGroup.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", staffAttendanceGroup },         // Data populated with JSON string
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

        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffAttendanceAllGroupAsync(GenerateReportDto reportDto)
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
                parameters.Add("@All", 1, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var totalAttendance = await _reportRepository.ExecuteStoredProcedureAsync("sp_Attendances_Report", parameters);

                // Check if the data is empty
                if (totalAttendance == null || !totalAttendance.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", totalAttendance },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateStaffAttendanceGroupEmptyReportAsync(GenerateReportDto reportDto)
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
                parameters.Add("@All", 2, DbType.Int32);
                //parameters.Add("@Position", reportDto.PositionId, DbType.String);

                // Call the repository to execute the stored procedure and fetch the data
                var totalAttendance = await _reportRepository.ExecuteStoredProcedureAsync("sp_Attendances_Report", parameters);

                // Check if the data is empty
                if (totalAttendance == null || !totalAttendance.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", totalAttendance },         // Data populated with JSON string
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
        public async Task<GenericResponse<Dictionary<string, object>>> GenerateTotalAttendanceZeroReportAsync(GenerateReportDto reportDto)
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
                var totalAttendance = await _reportRepository.ExecuteStoredProcedureAsync("sp_AttendancesSummeryZero_Report", parameters);

                // Check if the data is empty
                if (totalAttendance == null || !totalAttendance.Any())
                {
                    return new GenericResponse<Dictionary<string, object>>(true, "No data found for the report.", null);
                }

                // Prepare the dictionary response with the JSON data, setting other fields to null
                var responseData = new Dictionary<string, object>
                {
                    { "ReportData", totalAttendance },         // Data populated with JSON string
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
