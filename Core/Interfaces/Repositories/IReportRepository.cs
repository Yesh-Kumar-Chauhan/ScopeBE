using Core.DTOs.Report;
using Core.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report?> GetReportByIdAsync(long id);
        Task<Report> AddReportAsync(Report report);
        Task<Report?> UpdateReportAsync(Report report);
        Task<bool> DeleteReportAsync(long id);

        //reports
        Task<List<WelcomeLatter>> GetWelcomeLettersAsync(long? personelId, long? siteId, long? districtId, string? countryId, int? type);
        Task<IEnumerable<dynamic>> GetSiteInformationReportDataAsync(long? siteId, long? districtId, string? selections, int? type);
        Task<IEnumerable<dynamic>> GetScopeTimeSheetReportDataAsync(long? personelId, DateTime? startDate, DateTime? endDate, long? siteId, long? districtId, string? selections, int? type);
        Task<IEnumerable<dynamic>> GetScopeTimeSheetTotalReportDataAsync(long? personelId, DateTime? startDate, DateTime? endDate, long? siteId, long? districtId, string? selections, int? type);
        Task<IEnumerable<dynamic>> GenerateScopeEmployeeTimeSheetDataAsync(long? personelId, long? siteId, long? districtId, string? countryId, string? selections, int? type);
        Task<IEnumerable<dynamic>> GenerateStaffSalariesDataAsync(long? personelId, long? siteId, long? districtId, string? countryId, string? selections, int? type);
        Task<IEnumerable<dynamic>> GenerateStaffMatCprFaDataAsync(long? siteId, long? districtId, DateTime? startDate, DateTime? endDate, string? countryId, long? personelId, string? selections, int? type);
        Task<IEnumerable<dynamic>> GenerateStaffMaximumHourDataAsync(long? personelId, long? siteId, long? districtId, string? countryId, string? selections, int? type);
        Task<IEnumerable<dynamic>> ExecuteStoredProcedureAsync(string storedProcedureName, DynamicParameters parameters);

    }
}
