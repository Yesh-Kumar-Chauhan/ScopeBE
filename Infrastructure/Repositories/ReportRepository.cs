using Core.DTOs.Core.DTOs;
using Core.DTOs.Personel;
using Core.DTOs;
using Core.DTOs.Report;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public ReportRepository(AppDbContext context, IConfiguration configuration, string connectionStringName = "ProdConnection")
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(connectionStringName);
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetReportByIdAsync(long id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<Report> AddReportAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<Report?> UpdateReportAsync(Report report)
        {
            var existingReport = await _context.Reports.FindAsync(report.ID);
            if (existingReport == null)
            {
                return null;
            }

            _context.Entry(existingReport).CurrentValues.SetValues(report);
            await _context.SaveChangesAsync();

            return existingReport;
        }

        public async Task<bool> DeleteReportAsync(long id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return false;
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<WelcomeLatter>> GetWelcomeLettersAsync(long? personelId, long? siteId, long? districtId, string? countryId, int? type)
        {
            List<WelcomeLatter> items = new List<WelcomeLatter>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new SqlCommand("sp_WelcomeLatter_Report", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PersonID", SqlDbType.BigInt) { Value = personelId ?? (object)DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@SiteID", SqlDbType.BigInt) { Value = siteId ?? (object)DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@DistrictID", SqlDbType.BigInt) { Value = districtId ?? (object)DBNull.Value });
                    _ = cmd.Parameters.Add(new SqlParameter("@CountyID", SqlDbType.NVarChar) { Value = countryId ?? (object)DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar) { Value = type ?? (object)DBNull.Value });

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var welcomelatter = new WelcomeLatter
                            {
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                ZipCode = reader["ZipCode"].ToString(),
                                CPR = DateTime.TryParse(reader["CPR"].ToString(), out DateTime cprDate) ? (DateTime?)cprDate : null,
                                MatApp = DateTime.TryParse(reader["MatApp"].ToString(), out DateTime matAppDate) ? (DateTime?)matAppDate : null,
                                MatDate = DateTime.TryParse(reader["MatDate"].ToString(), out DateTime matDateDate) ? (DateTime?)matDateDate : null,
                                FirstAid = DateTime.TryParse(reader["FirstAid"].ToString(), out DateTime firstAidDate) ? (DateTime?)firstAidDate : null,
                                DistrictNameB = reader["DistrictNameB"].ToString(),
                                SiteNameB = reader["SiteNameB"].ToString(),
                                SitePositionB = reader["SitePositionB"].ToString(),
                                DistrictNameD = reader["DistrictNameD"].ToString(),
                                SiteNameD = reader["SiteNameD"].ToString(),
                                SitePositionD = reader["SitePositionD"].ToString(),
                                DistrictNameA = reader["DistrictNameA"].ToString(),
                                SiteNameA = reader["SiteNameA"].ToString(),
                                SitePositionA = reader["SitePositionA"].ToString(),
                                MON_1_B = reader["MON_1_B"].ToString(),
                                MON_1_E = reader["MON_1_E"].ToString(),
                                TUE_1_B = reader["TUE_1_B"].ToString(),
                                TUE_1_E = reader["TUE_1_E"].ToString(),
                                WED_1_B = reader["WED_1_B"].ToString(),
                                WED_1_E = reader["WED_1_E"].ToString(),
                                THU_1_B = reader["THU_1_B"].ToString(),
                                THU_1_E = reader["THU_1_E"].ToString(),
                                FRI_1_B = reader["FRI_1_B"].ToString(),
                                FRI_1_E = reader["FRI_1_E"].ToString(),
                                MON_2_B = reader["MON_2_B"].ToString(),
                                MON_2_E = reader["MON_2_E"].ToString(),
                                TUE_2_B = reader["TUE_2_B"].ToString(),
                                TUE_2_E = reader["TUE_2_E"].ToString(),
                                WED_2_B = reader["WED_2_B"].ToString(),
                                WED_2_E = reader["WED_2_E"].ToString(),
                                THU_2_B = reader["THU_2_B"].ToString(),
                                THU_2_E = reader["THU_2_E"].ToString(),
                                FRI_2_B = reader["FRI_2_B"].ToString(),
                                FRI_2_E = reader["FRI_2_E"].ToString(),
                                MON_3_B = reader["MON_3_B"].ToString(),
                                MON_3_E = reader["MON_3_E"].ToString(),
                                TUE_3_B = reader["TUE_3_B"].ToString(),
                                TUE_3_E = reader["TUE_3_E"].ToString(),
                                WED_3_B = reader["WED_3_B"].ToString(),
                                WED_3_E = reader["WED_3_E"].ToString(),
                                THU_3_B = reader["THU_3_B"].ToString(),
                                THU_3_E = reader["THU_3_E"].ToString(),
                                FRI_3_B = reader["FRI_3_B"].ToString(),
                                FRI_3_E = reader["FRI_3_E"].ToString(),
                                SEP_Rate_B = reader["SEP_Rate_B"].ToString(),
                                JAN_Rate_B = reader["JAN_Rate_B"].ToString(),
                                FEB_Rate_B = reader["FEB_Rate_B"].ToString(),
                                SEP_Rate_D = reader["SEP_Rate_D"].ToString(),
                                JAN_Rate_D = reader["JAN_Rate_D"].ToString(),
                                FEB_Rate_D = reader["FEB_Rate_D"].ToString(),
                                SEP_Rate_A = reader["SEP_Rate_A"].ToString(),
                                JAN_Rate_A = reader["JAN_Rate_A"].ToString(),
                                FEB_Rate_A = reader["FEB_Rate_A"].ToString(),
                                DOEMP = DateTime.TryParse(reader["DOEMP"].ToString(), out DateTime doempDate) ? (DateTime?)doempDate : null,
                                REHIRED = DateTime.TryParse(reader["REHIRED"].ToString(), out DateTime rehiredDate) ? (DateTime?)rehiredDate : null,
                                EffectiveDateBefore = DateTime.TryParse(reader["EffectiveDateBefore"].ToString(), out DateTime effDateBefore) ? (DateTime?)effDateBefore : null,
                                EffectiveDateDuring = DateTime.TryParse(reader["EffectiveDateDuring"].ToString(), out DateTime effDateDuring) ? (DateTime?)effDateDuring : null,
                                EffectiveDateAfter = DateTime.TryParse(reader["EffectiveDateAfter"].ToString(), out DateTime effDateAfter) ? (DateTime?)effDateAfter : null,
                                Comment = reader["Comment"].ToString(),
                                Foundations = DateTime.TryParse(reader["Foundations"].ToString(), out DateTime foundationsDate) ? (DateTime?)foundationsDate : null,
                            };


                            items.Add(welcomelatter);
                        }
                    }
                }
            }

            return items;
        }
        public async Task<IEnumerable<dynamic>> GetSiteInformationReportDataAsync(long? siteId, long? districtId, string siteType, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@SiteType", siteType, DbType.String);
                parameters.Add("@Type", type, DbType.Int32);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_SiteInformation_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }
        public async Task<IEnumerable<dynamic>> GetScopeTimeSheetReportDataAsync(long? personelId, DateTime? startDate, DateTime? endDate, long? siteId, long? districtId, string? selections, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", personelId, DbType.Int64);
                parameters.Add("@StartDate", startDate, DbType.DateTime);
                parameters.Add("@EndDate", endDate, DbType.DateTime);
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@SiteType", selections, DbType.String);
                parameters.Add("@Type", type, DbType.Int64);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_Timesheet_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }
        public async Task<IEnumerable<dynamic>> GetScopeTimeSheetTotalReportDataAsync(long? personelId, DateTime? startDate, DateTime? endDate, long? siteId, long? districtId, string? selections, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", personelId, DbType.Int64);
                parameters.Add("@StartDate", startDate, DbType.DateTime);
                parameters.Add("@EndDate", endDate, DbType.DateTime);
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@SiteType", selections, DbType.String);
                parameters.Add("@Type", type, DbType.Int64);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_Timesheet_Totals_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }
        public async Task<IEnumerable<dynamic>> GenerateScopeEmployeeTimeSheetDataAsync(
            long? personelId, long? siteId, long? districtId, string? countryId, string? selections, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", personelId, DbType.Int64);
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@CountyID", countryId, DbType.String);
                parameters.Add("@SiteType", selections, DbType.String);
                parameters.Add("@Type", type, DbType.Int64);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_EmployeeTimeSheet_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }

        public async Task<IEnumerable<dynamic>> GenerateStaffSalariesDataAsync(
            long? personelId, long? siteId, long? districtId, string? countryId, string? selections, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", personelId, DbType.Int64);
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@CountyID", countryId, DbType.String);
                parameters.Add("@SiteType", selections, DbType.String);
                parameters.Add("@Type", type, DbType.Int64);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_StaffSalaries_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }
        
        public async Task<IEnumerable<dynamic>> GenerateStaffMatCprFaDataAsync(
            long? siteId, long? districtId, DateTime? startDate, DateTime? endDate, string? countryId, long? personelId, string? selections, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@StartDate", startDate, DbType.DateTime);
                parameters.Add("@EndDate", endDate, DbType.DateTime);
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@PersonID", personelId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@CountyID", countryId, DbType.String);
                parameters.Add("@SiteType", selections, DbType.String);
                parameters.Add("@Type", type, DbType.Int64);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_StaffMatCprFA_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }

        public async Task<IEnumerable<dynamic>> GenerateStaffMaximumHourDataAsync(long? personelId, long? siteId, long? districtId, string? countryId, string? selections, int? type)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@PersonID", personelId, DbType.Int64);
                parameters.Add("@SiteID", siteId, DbType.Int64);
                parameters.Add("@DistrictID", districtId, DbType.Int64);
                parameters.Add("@CountyID", countryId, DbType.String);
                parameters.Add("@SiteType", selections, DbType.String);
                parameters.Add("@Type", type, DbType.Int64);

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    "sp_Stuff_MaxHours_Report",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result; // Returning IEnumerable<dynamic>
            }
        }

        public async Task<IEnumerable<dynamic>> ExecuteStoredProcedureAsync(string storedProcedureName, DynamicParameters parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Execute the stored procedure and return the raw results
                var result = await connection.QueryAsync(
                    storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
    }
}
