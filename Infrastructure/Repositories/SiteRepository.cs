using AutoMapper;
using Core.DTOs.Core.DTOs;
using Core.DTOs.Site;
using Core.DTOs.Sites;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Modals;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly string _connectionString;
        public SiteRepository(AppDbContext context,
            IMapper mapper,
            IConfiguration configuration,
            string connectionStringName = "ProdConnection"
           )
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(connectionStringName);
            _mapper = mapper;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        // For search and pagination
        public IQueryable<Site> GetAllSites()
        {
            return _context.Sites.AsQueryable();
        }

        // Optional: For simple retrieval without filtering
        public async Task<List<Site>> GetAllSitesAsync()
        {
            return await _context.Sites.ToListAsync();
        }

        public async Task<Site?> GetSiteByIdAsync(long id)
        {
            return await _context.Sites.FindAsync(id);
        }

        public async Task<Site> AddSiteAsync(Site site)
        {
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();
            return site;
        }

        public async Task<Site?> UpdateSiteAsync(Site site)
        {
            var existingSite = await _context.Sites.FindAsync(site.SiteID);
            if (existingSite == null)
            {
                return null;
            }

            _context.Entry(existingSite).CurrentValues.SetValues(site);
            await _context.SaveChangesAsync();
            return existingSite;
        }

        public async Task<bool> DeleteSiteAsync(long id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null)
            {
                return false;
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SiteByType>> GetSitesByTypeAsync(string? search, int type)
        {

            List<SiteByType> sites = new List<SiteByType>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_Sites_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@KeyWord", SqlDbType.VarChar) { Value = search });
                    command.Parameters.Add(new SqlParameter("@Operation", SqlDbType.Int) { Value = type });

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sites.Add(new SiteByType
                            {
                                SiteID = (long)(reader["SiteID"] ?? null),
                                SiteNumber = (long)(reader["SITE_NUM"] ?? null),
                                SiteName = reader["SITE_NAM"] != null ? reader["SITE_NAM"].ToString() : "",
                            });
                        }
                    }
                }
            }
            return sites;

        }

        public async Task<List<ExtendedSiteDto>> GetSitesByDistrictIdAsync(long districtId, long? districtNum, int operation)
        {
            List<ExtendedSiteDto> extendedSites = new List<ExtendedSiteDto>();


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_District_Sites_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@DistrictID", SqlDbType.BigInt) { Value = districtId == 0 ? DBNull.Value : districtId });
                    command.Parameters.Add(new SqlParameter("@DistrictNUM", SqlDbType.BigInt) { Value = districtNum ?? (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter("@Operation", SqlDbType.Int) { Value = operation });

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var site = new Site
                            {
                                SiteID = reader["SiteID"] != DBNull.Value ? Convert.ToInt64(reader["SiteID"]) : 0,
                                SITE_NUM = reader["SITE_NUM"] != DBNull.Value ? Convert.ToInt64(reader["SITE_NUM"]) : 0,
                                SITE_NAM = reader["SITE_NAM"] != DBNull.Value ? reader["SITE_NAM"].ToString() : null,
                                WHEN = reader["WHEN"] != DBNull.Value ? reader["WHEN"].ToString() : null,
                                ADDR1 = reader["ADDR1"] != DBNull.Value ? reader["ADDR1"].ToString() : null,
                                ADDR2 = reader["ADDR2"] != DBNull.Value ? reader["ADDR2"].ToString() : null,
                                ADDR3 = reader["ADDR3"] != DBNull.Value ? reader["ADDR3"].ToString() : null,
                                GRADE_LVLS = reader["GRADE_LVLS"] != DBNull.Value ? reader["GRADE_LVLS"].ToString() : null,
                                PHONE = reader["PHONE"] != DBNull.Value ? reader["PHONE"].ToString() : null,
                                PHONE_TYPE = reader["PHONE_TYPE"] != DBNull.Value ? reader["PHONE_TYPE"].ToString() : null,
                                TIME_AVAIL = reader["TIME_AVAIL"] != DBNull.Value ? reader["TIME_AVAIL"].ToString() : null,
                                ROOM_NO = reader["ROOM_NO"] != DBNull.Value ? reader["ROOM_NO"].ToString() : null,
                                START_TIME = reader["START_TIME"] != DBNull.Value ? reader["START_TIME"].ToString() : null,
                                STOP_TIME = reader["STOP_TIME"] != DBNull.Value ? reader["STOP_TIME"].ToString() : null,
                                START_DATE = reader["START_DATE"] != DBNull.Value ? (DateTime?)reader["START_DATE"] : null,
                                FULL = reader["FULL"] != DBNull.Value ? (decimal?)reader["FULL"] : null,
                                MIN = reader["MIN"] != DBNull.Value ? (decimal?)reader["MIN"] : null,
                                DAILY = reader["DAILY"] != DBNull.Value ? (decimal?)reader["DAILY"] : null,
                                AMPM = reader["AMPM"] != DBNull.Value ? (decimal?)reader["AMPM"] : null,
                                PERMIT = reader["PERMIT"] != DBNull.Value ? reader["PERMIT"].ToString() : null,
                                ISSUED = reader["ISSUED"] != DBNull.Value ? (DateTime?)reader["ISSUED"] : null,
                                STARTED = reader["STARTED"] != DBNull.Value ? (DateTime?)reader["STARTED"] : null,
                                CLOSED = reader["CLOSED"] != DBNull.Value ? (DateTime?)reader["CLOSED"] : null,
                                EXPIRES = reader["EXPIRES"] != DBNull.Value ? (DateTime?)reader["EXPIRES"] : null,
                                NOTES = reader["NOTES"] != DBNull.Value ? reader["NOTES"].ToString() : null,
                                AUTORENEW = reader["AUTORENEW"] != DBNull.Value ? Convert.ToBoolean(reader["AUTORENEW"]) : false,
                                ORIGCONTRC = reader["ORIGCONTRC"] != DBNull.Value ? (DateTime?)reader["ORIGCONTRC"] : null,
                                DIST_FEE = reader["DIST_FEE"] != DBNull.Value ? reader["DIST_FEE"].ToString() : null,
                                DSS_REP = reader["DSS_REP"] != DBNull.Value ? reader["DSS_REP"].ToString() : null,
                                DSS_FON = reader["DSS_FON"] != DBNull.Value ? reader["DSS_FON"].ToString() : null,
                                CAPACTIY = reader["CAPACTIY"] != DBNull.Value ? (int?)reader["CAPACTIY"] : null,
                                SITECAP = reader["SITECAP"] != DBNull.Value ? (int?)reader["SITECAP"] : null,
                                CLASS = reader["CLASS"] != DBNull.Value ? reader["CLASS"].ToString() : null,
                                COUNTY = reader["COUNTY"] != DBNull.Value ? reader["COUNTY"].ToString() : null,
                                PRINCIPAL = reader["PRINCIPAL"] != DBNull.Value ? reader["PRINCIPAL"].ToString() : null,
                                SCHFONE = reader["SCHFONE"] != DBNull.Value ? reader["SCHFONE"].ToString() : null,
                                CLOSINGS = reader["CLOSINGS"] != DBNull.Value ? reader["CLOSINGS"].ToString() : null,
                                EMERGENCY = reader["EMERGENCY"] != DBNull.Value ? reader["EMERGENCY"].ToString() : null,
                                PADR1 = reader["PADR1"] != DBNull.Value ? reader["PADR1"].ToString() : null,
                                PADR2 = reader["PADR2"] != DBNull.Value ? reader["PADR2"].ToString() : null,
                                PADR3 = reader["PADR3"] != DBNull.Value ? reader["PADR3"].ToString() : null,
                                PPHONE = reader["PPHONE"] != DBNull.Value ? reader["PPHONE"].ToString() : null,
                                FADR1 = reader["FADR1"] != DBNull.Value ? reader["FADR1"].ToString() : null,
                                FADR2 = reader["FADR2"] != DBNull.Value ? reader["FADR2"].ToString() : null,
                                FADR3 = reader["FADR3"] != DBNull.Value ? reader["FADR3"].ToString() : null,
                                FPHONE = reader["FPHONE"] != DBNull.Value ? reader["FPHONE"].ToString() : null,
                                EADR1 = reader["EADR1"] != DBNull.Value ? reader["EADR1"].ToString() : null,
                                EADR2 = reader["EADR2"] != DBNull.Value ? reader["EADR2"].ToString() : null,
                                EADR3 = reader["EADR3"] != DBNull.Value ? reader["EADR3"].ToString() : null,
                                EPHONE = reader["EPHONE"] != DBNull.Value ? reader["EPHONE"].ToString() : null,
                                VISIT_ID = reader["VISIT_ID"] != DBNull.Value ? (decimal?)reader["VISIT_ID"] : null,
                                WL_DATE = reader["WL_DATE"] != DBNull.Value ? reader["WL_DATE"].ToString() : null,
                                WL_COUNT = reader["WL_COUNT"] != DBNull.Value ? (decimal?)reader["WL_COUNT"] : null,
                                PC_DATE = reader["PC_DATE"] != DBNull.Value ? (DateTime?)reader["PC_DATE"] : null,
                                PC_AMT = reader["PC_AMT"] != DBNull.Value ? (decimal?)reader["PC_AMT"] : null,
                                TRANSPORT = reader["TRANSPORT"] != DBNull.Value ? reader["TRANSPORT"].ToString() : null,
                                TPPHONE = reader["TPPHONE"] != DBNull.Value ? reader["TPPHONE"].ToString() : null,
                                TEL_VOL = reader["TEL_VOL"] != DBNull.Value ? reader["TEL_VOL"].ToString() : null,
                                DT_VOL = reader["DT_VOL"] != DBNull.Value ? reader["DT_VOL"].ToString() : null,
                                SECURITY = reader["SECURITY"] != DBNull.Value ? reader["SECURITY"].ToString() : null,
                                SECPHONE = reader["SECPHONE"] != DBNull.Value ? reader["SECPHONE"].ToString() : null,
                                SAFEPLACE = reader["SAFEPLACE"] != DBNull.Value ? reader["SAFEPLACE"].ToString() : null,
                                LOCKDOWN = reader["LOCKDOWN"] != DBNull.Value ? reader["LOCKDOWN"].ToString() : null,
                                RELOCROOM1 = reader["RELOCROOM1"] != DBNull.Value ? reader["RELOCROOM1"].ToString() : null,
                                RELOCROOM2 = reader["RELOCROOM2"] != DBNull.Value ? reader["RELOCROOM2"].ToString() : null,
                                LNDLNLOC = reader["LNDLNLOC"] != DBNull.Value ? reader["LNDLNLOC"].ToString() : null,
                                ADDLOC = reader["ADDLOC"] != DBNull.Value ? reader["ADDLOC"].ToString() : null,
                                ATTNOTE = reader["ATTNOTE"] != DBNull.Value ? reader["ATTNOTE"].ToString() : null,
                                SRCSCHLS = reader["SRCSCHLS"] != DBNull.Value ? reader["SRCSCHLS"].ToString() : null,
                                RSPNSBL = reader["RSPNSBL"] != DBNull.Value ? reader["RSPNSBL"].ToString() : null,
                                TWONAMES = reader["TWONAMES"] != DBNull.Value ? reader["TWONAMES"].ToString() : null,
                                HCPSENT = reader["HCPSENT"] != DBNull.Value ? (DateTime?)reader["HCPSENT"] : null,
                                HCPAPPR = reader["HCPAPPR"] != DBNull.Value ? (DateTime?)reader["HCPAPPR"] : null,
                                ENROLLMENT = reader["ENROLLMENT"] != DBNull.Value ? (decimal?)reader["ENROLLMENT"] : null,
                                COEXP = reader["COEXP"] != DBNull.Value ? reader["COEXP"].ToString() : null,
                                WAIVEREXP = reader["WAIVEREXP"] != DBNull.Value ? (DateTime?)reader["WAIVEREXP"] : null,
                                NURSEVISIT = reader["NURSEVISIT"] != DBNull.Value ? (DateTime?)reader["NURSEVISIT"] : null,
                                EPIPEN = reader["EPIPEN"] != DBNull.Value && Convert.ToBoolean(reader["EPIPEN"]),
                                INHALER = reader["INHALER"] != DBNull.Value && Convert.ToBoolean(reader["INHALER"]),
                                BENADRYL = reader["BENADRYL"] != DBNull.Value && Convert.ToBoolean(reader["BENADRYL"]),
                                HCPREV = reader["HCPREV"] != DBNull.Value ? (DateTime?)reader["HCPREV"] : null,
                                NURSEVISI2 = reader["NURSEVISI2"] != DBNull.Value ? (DateTime?)reader["NURSEVISI2"] : null,
                                SFAX = reader["SFAX"] != DBNull.Value ? reader["SFAX"].ToString() : null,
                                ADDLOC2 = reader["ADDLOC2"] != DBNull.Value ? reader["ADDLOC2"].ToString() : null,
                                ADDLOC3 = reader["ADDLOC3"] != DBNull.Value ? reader["ADDLOC3"].ToString() : null,
                                ADDLOC4 = reader["ADDLOC4"] != DBNull.Value ? reader["ADDLOC4"].ToString() : null,
                                NURSEVISI3 = reader["NURSEVISI3"] != DBNull.Value ? (DateTime?)reader["NURSEVISI3"] : null,
                                SCOPEFAX = reader["SCOPEFAX"] != DBNull.Value ? reader["SCOPEFAX"].ToString() : null,
                                ASCAP1 = reader["ASCAP1"] != DBNull.Value ? (int?)reader["ASCAP1"] : null,
                                ASCAP2 = reader["ASCAP2"] != DBNull.Value ? (int?)reader["ASCAP2"] : null,
                                ASCAP3 = reader["ASCAP3"] != DBNull.Value ? (int?)reader["ASCAP3"] : null,
                                ASCAP4 = reader["ASCAP4"] != DBNull.Value ? (int?)reader["ASCAP4"] : null,
                                PEMAIL = reader["PEMAIL"] != DBNull.Value ? reader["PEMAIL"].ToString() : null,
                                PK = reader["PK"] != DBNull.Value && Convert.ToBoolean(reader["PK"]),
                                UPK = reader["UPK"] != DBNull.Value && Convert.ToBoolean(reader["UPK"]),
                                EADR21 = reader["EADR21"] != DBNull.Value ? reader["EADR21"].ToString() : null,
                                EADR22 = reader["EADR22"] != DBNull.Value ? reader["EADR22"].ToString() : null,
                                EADR23 = reader["EADR23"] != DBNull.Value ? reader["EADR23"].ToString() : null,
                                EPHONE2 = reader["EPHONE2"] != DBNull.Value ? reader["EPHONE2"].ToString() : null,
                                AMBPHONE = reader["AMBPHONE"] != DBNull.Value ? reader["AMBPHONE"].ToString() : null,
                                ADDEMGINFO = reader["ADDEMGINFO"] != DBNull.Value ? reader["ADDEMGINFO"].ToString() : null,
                                ROOM_NO2 = reader["ROOM_NO2"] != DBNull.Value ? reader["ROOM_NO2"].ToString() : null,
                                ROOM_NO3 = reader["ROOM_NO3"] != DBNull.Value ? reader["ROOM_NO3"].ToString() : null,
                                ROOM_NO4 = reader["ROOM_NO4"] != DBNull.Value ? reader["ROOM_NO4"].ToString() : null,
                                CAP1 = reader["CAP1"] != DBNull.Value ? (int?)reader["CAP1"] : null,
                                CAP2 = reader["CAP2"] != DBNull.Value ? (int?)reader["CAP2"] : null,
                                CAP3 = reader["CAP3"] != DBNull.Value ? (int?)reader["CAP3"] : null,
                                CAP4 = reader["CAP4"] != DBNull.Value ? (int?)reader["CAP4"] : null,
                                OSSPLACE = reader["OSSPLACE"] != DBNull.Value ? reader["OSSPLACE"].ToString() : null,
                                ScopeEmail = reader["ScopeEmail"] != DBNull.Value ? reader["ScopeEmail"].ToString() : null,
                                ScopePassword = reader["ScopePassword"] != DBNull.Value ? reader["ScopePassword"].ToString() : null,
                                MidPoint = reader["MidPoint"] != DBNull.Value ? reader["MidPoint"].ToString() : null,
                                StaffingAssistantName = reader["StaffingAssistantName"] != DBNull.Value ? reader["StaffingAssistantName"].ToString() : null,
                                StaffingAssistantPhone = reader["StaffingAssistantPhone"] != DBNull.Value ? reader["StaffingAssistantPhone"].ToString() : null,
                                StaffingAssistantFax = reader["StaffingAssistantFax"] != DBNull.Value ? reader["StaffingAssistantFax"].ToString() : null,
                                StaffingAssistantEmail = reader["StaffingAssistantEmail"] != DBNull.Value ? reader["StaffingAssistantEmail"].ToString() : null,
                                StaffingAssistantOthers = reader["StaffingAssistantOthers"] != DBNull.Value ? reader["StaffingAssistantOthers"].ToString() : null,
                                StaffingAssistantExt = reader["StaffingAssistantExt"] != DBNull.Value ? reader["StaffingAssistantExt"].ToString() : null,
                                StaffingAssistantForms = reader["StaffingAssistantForms"] != DBNull.Value ? reader["StaffingAssistantForms"].ToString() : null,
                                FoodContactName = reader["FoodContactName"] != DBNull.Value ? reader["FoodContactName"].ToString() : null,
                                FoodContactPhone = reader["FoodContactPhone"] != DBNull.Value ? reader["FoodContactPhone"].ToString() : null,
                                FoodContactFax = reader["FoodContactFax"] != DBNull.Value ? reader["FoodContactFax"].ToString() : null,
                                FoodContactEmail = reader["FoodContactEmail"] != DBNull.Value ? reader["FoodContactEmail"].ToString() : null,
                                FoodContactOthers = reader["FoodContactOthers"] != DBNull.Value ? reader["FoodContactOthers"].ToString() : null,
                                FoodContactExt = reader["FoodContactExt"] != DBNull.Value ? reader["FoodContactExt"].ToString() : null,
                                FoodContactForms = reader["FoodContactForms"] != DBNull.Value ? reader["FoodContactForms"].ToString() : null,
                                SupplyContactName = reader["SupplyContactName"] != DBNull.Value ? reader["SupplyContactName"].ToString() : null,
                                SupplyContactPhone = reader["SupplyContactPhone"] != DBNull.Value ? reader["SupplyContactPhone"].ToString() : null,
                                SupplyContactFax = reader["SupplyContactFax"] != DBNull.Value ? reader["SupplyContactFax"].ToString() : null,
                                SupplyContactEmail = reader["SupplyContactEmail"] != DBNull.Value ? reader["SupplyContactEmail"].ToString() : null,
                                SupplyContactOthers = reader["SupplyContactOthers"] != DBNull.Value ? reader["SupplyContactOthers"].ToString() : null,
                                SupplyContactExt = reader["SupplyContactExt"] != DBNull.Value ? reader["SupplyContactExt"].ToString() : null,
                                SupplyContactForms = reader["SupplyContactForms"] != DBNull.Value ? reader["SupplyContactForms"].ToString() : null,
                                PettyCashSpecialEventContactName = reader["PettyCashSpecialEventContactName"] != DBNull.Value ? reader["PettyCashSpecialEventContactName"].ToString() : null,
                                PettyCashSpecialEventContactPhone = reader["PettyCashSpecialEventContactPhone"] != DBNull.Value ? reader["PettyCashSpecialEventContactPhone"].ToString() : null,
                                PettyCashSpecialEventContactFax = reader["PettyCashSpecialEventContactFax"] != DBNull.Value ? reader["PettyCashSpecialEventContactFax"].ToString() : null,
                                PettyCashSpecialEventContactEmail = reader["PettyCashSpecialEventContactEmail"] != DBNull.Value ? reader["PettyCashSpecialEventContactEmail"].ToString() : null,
                                PettyCashSpecialEventContactOthers = reader["PettyCashSpecialEventContactOthers"] != DBNull.Value ? reader["PettyCashSpecialEventContactOthers"].ToString() : null,
                                PettyCashSpecialEventContactExt = reader["PettyCashSpecialEventContactExt"] != DBNull.Value ? reader["PettyCashSpecialEventContactExt"].ToString() : null,
                                PettyCashSpecialEventContactForms = reader["PettyCashSpecialEventContactForms"] != DBNull.Value ? reader["PettyCashSpecialEventContactForms"].ToString() : null,
                                FieldSupervisorName = reader["FieldSupervisorName"] != DBNull.Value ? reader["FieldSupervisorName"].ToString() : null,
                                FieldSupervisorPhone = reader["FieldSupervisorPhone"] != DBNull.Value ? reader["FieldSupervisorPhone"].ToString() : null,
                                FieldSupervisorFax = reader["FieldSupervisorFax"] != DBNull.Value ? reader["FieldSupervisorFax"].ToString() : null,
                                FieldSupervisorEmail = reader["FieldSupervisorEmail"] != DBNull.Value ? reader["FieldSupervisorEmail"].ToString() : null,
                                FieldSupervisorOthers = reader["FieldSupervisorOthers"] != DBNull.Value ? reader["FieldSupervisorOthers"].ToString() : null,
                                FieldSupervisorExt = reader["FieldSupervisorExt"] != DBNull.Value ? reader["FieldSupervisorExt"].ToString() : null,
                                FieldSupervisorForms = reader["FieldSupervisorForms"] != DBNull.Value ? reader["FieldSupervisorForms"].ToString() : null,
                                FieldTrainerName = reader["FieldTrainerName"] != DBNull.Value ? reader["FieldTrainerName"].ToString() : null,
                                FieldTrainerPhone = reader["FieldTrainerPhone"] != DBNull.Value ? reader["FieldTrainerPhone"].ToString() : null,
                                FieldTrainerFax = reader["FieldTrainerFax"] != DBNull.Value ? reader["FieldTrainerFax"].ToString() : null,
                                FieldTrainerEmail = reader["FieldTrainerEmail"] != DBNull.Value ? reader["FieldTrainerEmail"].ToString() : null,
                                FieldTrainerOthers = reader["FieldTrainerOthers"] != DBNull.Value ? reader["FieldTrainerOthers"].ToString() : null,
                                FieldTrainerExt = reader["FieldTrainerExt"] != DBNull.Value ? reader["FieldTrainerExt"].ToString() : null,
                                FieldTrainerForms = reader["FieldTrainerForms"] != DBNull.Value ? reader["FieldTrainerForms"].ToString() : null,
                                HealthCareConsultantName = reader["HealthCareConsultantName"] != DBNull.Value ? reader["HealthCareConsultantName"].ToString() : null,
                                HealthCareConsultantPhone = reader["HealthCareConsultantPhone"] != DBNull.Value ? reader["HealthCareConsultantPhone"].ToString() : null,
                                HealthCareConsultantFax = reader["HealthCareConsultantFax"] != DBNull.Value ? reader["HealthCareConsultantFax"].ToString() : null,
                                HealthCareConsultantEmail = reader["HealthCareConsultantEmail"] != DBNull.Value ? reader["HealthCareConsultantEmail"].ToString() : null,
                                HealthCareConsultantOthers = reader["HealthCareConsultantOthers"] != DBNull.Value ? reader["HealthCareConsultantOthers"].ToString() : null,
                                HealthCareConsultantExt = reader["HealthCareConsultantExt"] != DBNull.Value ? reader["HealthCareConsultantExt"].ToString() : null,
                                HealthCareConsultantForms = reader["HealthCareConsultantForms"] != DBNull.Value ? reader["HealthCareConsultantForms"].ToString() : null,
                                RegistarName = reader["RegistarName"] != DBNull.Value ? reader["RegistarName"].ToString() : null,
                                RegistarPhone = reader["RegistarPhone"] != DBNull.Value ? reader["RegistarPhone"].ToString() : null,
                                RegistarFax = reader["RegistarFax"] != DBNull.Value ? reader["RegistarFax"].ToString() : null,
                                RegistarEmail = reader["RegistarEmail"] != DBNull.Value ? reader["RegistarEmail"].ToString() : null,
                                RegistarOthers = reader["RegistarOthers"] != DBNull.Value ? reader["RegistarOthers"].ToString() : null,
                                RegistarExt = reader["RegistarExt"] != DBNull.Value ? reader["RegistarExt"].ToString() : null,
                                RegistarForms = reader["RegistarForms"] != DBNull.Value ? reader["RegistarForms"].ToString() : null,
                                AccountBillingName = reader["AccountBillingName"] != DBNull.Value ? reader["AccountBillingName"].ToString() : null,
                                AccountBillingPhone = reader["AccountBillingPhone"] != DBNull.Value ? reader["AccountBillingPhone"].ToString() : null,
                                AccountBillingFax = reader["AccountBillingFax"] != DBNull.Value ? reader["AccountBillingFax"].ToString() : null,
                                AccountBillingEmail = reader["AccountBillingEmail"] != DBNull.Value ? reader["AccountBillingEmail"].ToString() : null,
                                AccountBillingOthers = reader["AccountBillingOthers"] != DBNull.Value ? reader["AccountBillingOthers"].ToString() : null,
                                AccountBillingExt = reader["AccountBillingExt"] != DBNull.Value ? reader["AccountBillingExt"].ToString() : null,
                                AccountBillingForms = reader["AccountBillingForms"] != DBNull.Value ? reader["AccountBillingForms"].ToString() : null,
                                SubstitutesName = reader["SubstitutesName"] != DBNull.Value ? reader["SubstitutesName"].ToString() : null,
                                SubstitutesPhone = reader["SubstitutesPhone"] != DBNull.Value ? reader["SubstitutesPhone"].ToString() : null,
                                SubstitutesFax = reader["SubstitutesFax"] != DBNull.Value ? reader["SubstitutesFax"].ToString() : null,
                                SubstitutesEmail = reader["SubstitutesEmail"] != DBNull.Value ? reader["SubstitutesEmail"].ToString() : null,
                                SubstitutesOthers = reader["SubstitutesOthers"] != DBNull.Value ? reader["SubstitutesOthers"].ToString() : null,
                                SubstitutesExt = reader["SubstitutesExt"] != DBNull.Value ? reader["SubstitutesExt"].ToString() : null,
                                SubstitutesForms = reader["SubstitutesForms"] != DBNull.Value ? reader["SubstitutesForms"].ToString() : null,
                                SuppliesName = reader["SuppliesName"] != DBNull.Value ? reader["SuppliesName"].ToString() : null,
                                SuppliesPhone = reader["SuppliesPhone"] != DBNull.Value ? reader["SuppliesPhone"].ToString() : null,
                                SuppliesFax = reader["SuppliesFax"] != DBNull.Value ? reader["SuppliesFax"].ToString() : null,
                                SuppliesEmail = reader["SuppliesEmail"] != DBNull.Value ? reader["SuppliesEmail"].ToString() : null,
                                SuppliesOthers = reader["SuppliesOthers"] != DBNull.Value ? reader["SuppliesOthers"].ToString() : null,
                                SuppliesExt = reader["SuppliesExt"] != DBNull.Value ? reader["SuppliesExt"].ToString() : null,
                                SuppliesForms = reader["SuppliesForms"] != DBNull.Value ? reader["SuppliesForms"].ToString() : null,
                                AccountAssistantOthers = reader["AccountAssistantOthers"] != DBNull.Value ? reader["AccountAssistantOthers"].ToString() : null,
                                Type = reader["Type"] != DBNull.Value ? (long?)reader["Type"] : null,
                                Priority = reader["Priority"] != DBNull.Value ? (long?)reader["Priority"] : null,
                                SCOPEDSSName = reader["SCOPEDSSName"] != DBNull.Value ? reader["SCOPEDSSName"].ToString() : null,
                                SCOPEDSSPhone = reader["SCOPEDSSPhone"] != DBNull.Value ? reader["SCOPEDSSPhone"].ToString() : null,
                                SCOPEDSSFax = reader["SCOPEDSSFax"] != DBNull.Value ? reader["SCOPEDSSFax"].ToString() : null,
                                SCOPEDSSEmail = reader["SCOPEDSSEmail"] != DBNull.Value ? reader["SCOPEDSSEmail"].ToString() : null,
                                SCOPEDSSOthers = reader["SCOPEDSSOthers"] != DBNull.Value ? reader["SCOPEDSSOthers"].ToString() : null,
                                SCOPEDSSExt = reader["SCOPEDSSExt"] != DBNull.Value ? reader["SCOPEDSSExt"].ToString() : null,
                                SCOPEDSSForms = reader["SCOPEDSSForms"] != DBNull.Value ? reader["SCOPEDSSForms"].ToString() : null,
                                PresentersName = reader["PresentersName"] != DBNull.Value ? reader["PresentersName"].ToString() : null,
                                PresentersPhone = reader["PresentersPhone"] != DBNull.Value ? reader["PresentersPhone"].ToString() : null,
                                PresentersFax = reader["PresentersFax"] != DBNull.Value ? reader["PresentersFax"].ToString() : null,
                                PresentersEmail = reader["PresentersEmail"] != DBNull.Value ? reader["PresentersEmail"].ToString() : null,
                                PresentersOthers = reader["PresentersOthers"] != DBNull.Value ? reader["PresentersOthers"].ToString() : null,
                                PresentersExt = reader["PresentersExt"] != DBNull.Value ? reader["PresentersExt"].ToString() : null,
                                PresentersForms = reader["PresentersForms"] != DBNull.Value ? reader["PresentersForms"].ToString() : null,
                                AccountAssistantName = reader["AccountAssistantName"] != DBNull.Value ? reader["AccountAssistantName"].ToString() : null,
                                AccountAssistantPhone = reader["AccountAssistantPhone"] != DBNull.Value ? reader["AccountAssistantPhone"].ToString() : null,
                                AccountAssistantFax = reader["AccountAssistantFax"] != DBNull.Value ? reader["AccountAssistantFax"].ToString() : null,
                                AccountAssistantEmail = reader["AccountAssistantEmail"] != DBNull.Value ? reader["AccountAssistantEmail"].ToString() : null,
                            };

                            // Map to ExtendedSiteDto using AutoMapper
                            var siteDto = _mapper.Map<SiteDto>(site);
                            var extendedSiteDto = _mapper.Map<ExtendedSiteDto>(siteDto);

                            // Manually set the FullName property since it doesn't exist in Site
                            extendedSiteDto.FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : null;

                            extendedSites.Add(extendedSiteDto);
                        }
                    }
                }
            }

            return extendedSites;
        }


        public async Task<List<Site>> GetSitesByOperationAsync(string keyword, int operation)
        {
            List<Site> sites = new List<Site>();


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_Sites_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@KeyWord", SqlDbType.VarChar) { Value = keyword });
                    command.Parameters.Add(new SqlParameter("@Operation", SqlDbType.Int) { Value = operation });
                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var site = new Site
                            {
                                SiteID = reader["SiteID"] != DBNull.Value ? Convert.ToInt64(reader["SiteID"]) : 0,
                                SITE_NUM = reader["SITE_NUM"] != DBNull.Value ? Convert.ToInt64(reader["SITE_NUM"]) : 0,
                                SITE_NAM = reader["SITE_NAM"] != DBNull.Value ? reader["SITE_NAM"].ToString() : null,
                                WHEN = reader["WHEN"] != DBNull.Value ? reader["WHEN"].ToString() : null,
                                ADDR1 = reader["ADDR1"] != DBNull.Value ? reader["ADDR1"].ToString() : null,
                                ADDR2 = reader["ADDR2"] != DBNull.Value ? reader["ADDR2"].ToString() : null,
                                ADDR3 = reader["ADDR3"] != DBNull.Value ? reader["ADDR3"].ToString() : null,
                                GRADE_LVLS = reader["GRADE_LVLS"] != DBNull.Value ? reader["GRADE_LVLS"].ToString() : null,
                                PHONE = reader["PHONE"] != DBNull.Value ? reader["PHONE"].ToString() : null,
                                PHONE_TYPE = reader["PHONE_TYPE"] != DBNull.Value ? reader["PHONE_TYPE"].ToString() : null,
                                TIME_AVAIL = reader["TIME_AVAIL"] != DBNull.Value ? reader["TIME_AVAIL"].ToString() : null,
                                ROOM_NO = reader["ROOM_NO"] != DBNull.Value ? reader["ROOM_NO"].ToString() : null,
                                START_TIME = reader["START_TIME"] != DBNull.Value ? reader["START_TIME"].ToString() : null,
                                STOP_TIME = reader["STOP_TIME"] != DBNull.Value ? reader["STOP_TIME"].ToString() : null,
                                START_DATE = reader["START_DATE"] != DBNull.Value ? (DateTime?)reader["START_DATE"] : null,
                                FULL = reader["FULL"] != DBNull.Value ? (decimal?)reader["FULL"] : null,
                                MIN = reader["MIN"] != DBNull.Value ? (decimal?)reader["MIN"] : null,
                                DAILY = reader["DAILY"] != DBNull.Value ? (decimal?)reader["DAILY"] : null,
                                AMPM = reader["AMPM"] != DBNull.Value ? (decimal?)reader["AMPM"] : null,
                                PERMIT = reader["PERMIT"] != DBNull.Value ? reader["PERMIT"].ToString() : null,
                                ISSUED = reader["ISSUED"] != DBNull.Value ? (DateTime?)reader["ISSUED"] : null,
                                STARTED = reader["STARTED"] != DBNull.Value ? (DateTime?)reader["STARTED"] : null,
                                CLOSED = reader["CLOSED"] != DBNull.Value ? (DateTime?)reader["CLOSED"] : null,
                                EXPIRES = reader["EXPIRES"] != DBNull.Value ? (DateTime?)reader["EXPIRES"] : null,
                                NOTES = reader["NOTES"] != DBNull.Value ? reader["NOTES"].ToString() : null,
                                AUTORENEW = reader["AUTORENEW"] != DBNull.Value ? Convert.ToBoolean(reader["AUTORENEW"]) : false,
                                ORIGCONTRC = reader["ORIGCONTRC"] != DBNull.Value ? (DateTime?)reader["ORIGCONTRC"] : null,
                                DIST_FEE = reader["DIST_FEE"] != DBNull.Value ? reader["DIST_FEE"].ToString() : null,
                                DSS_REP = reader["DSS_REP"] != DBNull.Value ? reader["DSS_REP"].ToString() : null,
                                DSS_FON = reader["DSS_FON"] != DBNull.Value ? reader["DSS_FON"].ToString() : null,
                                CAPACTIY = reader["CAPACTIY"] != DBNull.Value ? (int?)reader["CAPACTIY"] : null,
                                SITECAP = reader["SITECAP"] != DBNull.Value ? (int?)reader["SITECAP"] : null,
                                CLASS = reader["CLASS"] != DBNull.Value ? reader["CLASS"].ToString() : null,
                                COUNTY = reader["COUNTY"] != DBNull.Value ? reader["COUNTY"].ToString() : null,
                                PRINCIPAL = reader["PRINCIPAL"] != DBNull.Value ? reader["PRINCIPAL"].ToString() : null,
                                SCHFONE = reader["SCHFONE"] != DBNull.Value ? reader["SCHFONE"].ToString() : null,
                                CLOSINGS = reader["CLOSINGS"] != DBNull.Value ? reader["CLOSINGS"].ToString() : null,
                                EMERGENCY = reader["EMERGENCY"] != DBNull.Value ? reader["EMERGENCY"].ToString() : null,
                                PADR1 = reader["PADR1"] != DBNull.Value ? reader["PADR1"].ToString() : null,
                                PADR2 = reader["PADR2"] != DBNull.Value ? reader["PADR2"].ToString() : null,
                                PADR3 = reader["PADR3"] != DBNull.Value ? reader["PADR3"].ToString() : null,
                                PPHONE = reader["PPHONE"] != DBNull.Value ? reader["PPHONE"].ToString() : null,
                                FADR1 = reader["FADR1"] != DBNull.Value ? reader["FADR1"].ToString() : null,
                                FADR2 = reader["FADR2"] != DBNull.Value ? reader["FADR2"].ToString() : null,
                                FADR3 = reader["FADR3"] != DBNull.Value ? reader["FADR3"].ToString() : null,
                                FPHONE = reader["FPHONE"] != DBNull.Value ? reader["FPHONE"].ToString() : null,
                                EADR1 = reader["EADR1"] != DBNull.Value ? reader["EADR1"].ToString() : null,
                                EADR2 = reader["EADR2"] != DBNull.Value ? reader["EADR2"].ToString() : null,
                                EADR3 = reader["EADR3"] != DBNull.Value ? reader["EADR3"].ToString() : null,
                                EPHONE = reader["EPHONE"] != DBNull.Value ? reader["EPHONE"].ToString() : null,
                                VISIT_ID = reader["VISIT_ID"] != DBNull.Value ? (decimal?)reader["VISIT_ID"] : null,
                                WL_DATE = reader["WL_DATE"] != DBNull.Value ? reader["WL_DATE"].ToString() : null,
                                WL_COUNT = reader["WL_COUNT"] != DBNull.Value ? (decimal?)reader["WL_COUNT"] : null,
                                PC_DATE = reader["PC_DATE"] != DBNull.Value ? (DateTime?)reader["PC_DATE"] : null,
                                PC_AMT = reader["PC_AMT"] != DBNull.Value ? (decimal?)reader["PC_AMT"] : null,
                                TRANSPORT = reader["TRANSPORT"] != DBNull.Value ? reader["TRANSPORT"].ToString() : null,
                                TPPHONE = reader["TPPHONE"] != DBNull.Value ? reader["TPPHONE"].ToString() : null,
                                TEL_VOL = reader["TEL_VOL"] != DBNull.Value ? reader["TEL_VOL"].ToString() : null,
                                DT_VOL = reader["DT_VOL"] != DBNull.Value ? reader["DT_VOL"].ToString() : null,
                                SECURITY = reader["SECURITY"] != DBNull.Value ? reader["SECURITY"].ToString() : null,
                                SECPHONE = reader["SECPHONE"] != DBNull.Value ? reader["SECPHONE"].ToString() : null,
                                SAFEPLACE = reader["SAFEPLACE"] != DBNull.Value ? reader["SAFEPLACE"].ToString() : null,
                                LOCKDOWN = reader["LOCKDOWN"] != DBNull.Value ? reader["LOCKDOWN"].ToString() : null,
                                RELOCROOM1 = reader["RELOCROOM1"] != DBNull.Value ? reader["RELOCROOM1"].ToString() : null,
                                RELOCROOM2 = reader["RELOCROOM2"] != DBNull.Value ? reader["RELOCROOM2"].ToString() : null,
                                LNDLNLOC = reader["LNDLNLOC"] != DBNull.Value ? reader["LNDLNLOC"].ToString() : null,
                                ADDLOC = reader["ADDLOC"] != DBNull.Value ? reader["ADDLOC"].ToString() : null,
                                ATTNOTE = reader["ATTNOTE"] != DBNull.Value ? reader["ATTNOTE"].ToString() : null,
                                SRCSCHLS = reader["SRCSCHLS"] != DBNull.Value ? reader["SRCSCHLS"].ToString() : null,
                                RSPNSBL = reader["RSPNSBL"] != DBNull.Value ? reader["RSPNSBL"].ToString() : null,
                                TWONAMES = reader["TWONAMES"] != DBNull.Value ? reader["TWONAMES"].ToString() : null,
                                HCPSENT = reader["HCPSENT"] != DBNull.Value ? (DateTime?)reader["HCPSENT"] : null,
                                HCPAPPR = reader["HCPAPPR"] != DBNull.Value ? (DateTime?)reader["HCPAPPR"] : null,
                                ENROLLMENT = reader["ENROLLMENT"] != DBNull.Value ? (decimal?)reader["ENROLLMENT"] : null,
                                COEXP = reader["COEXP"] != DBNull.Value ? reader["COEXP"].ToString() : null,
                                WAIVEREXP = reader["WAIVEREXP"] != DBNull.Value ? (DateTime?)reader["WAIVEREXP"] : null,
                                NURSEVISIT = reader["NURSEVISIT"] != DBNull.Value ? (DateTime?)reader["NURSEVISIT"] : null,
                                EPIPEN = reader["EPIPEN"] != DBNull.Value && Convert.ToBoolean(reader["EPIPEN"]),
                                INHALER = reader["INHALER"] != DBNull.Value && Convert.ToBoolean(reader["INHALER"]),
                                BENADRYL = reader["BENADRYL"] != DBNull.Value && Convert.ToBoolean(reader["BENADRYL"]),
                                HCPREV = reader["HCPREV"] != DBNull.Value ? (DateTime?)reader["HCPREV"] : null,
                                NURSEVISI2 = reader["NURSEVISI2"] != DBNull.Value ? (DateTime?)reader["NURSEVISI2"] : null,
                                SFAX = reader["SFAX"] != DBNull.Value ? reader["SFAX"].ToString() : null,
                                ADDLOC2 = reader["ADDLOC2"] != DBNull.Value ? reader["ADDLOC2"].ToString() : null,
                                ADDLOC3 = reader["ADDLOC3"] != DBNull.Value ? reader["ADDLOC3"].ToString() : null,
                                ADDLOC4 = reader["ADDLOC4"] != DBNull.Value ? reader["ADDLOC4"].ToString() : null,
                                NURSEVISI3 = reader["NURSEVISI3"] != DBNull.Value ? (DateTime?)reader["NURSEVISI3"] : null,
                                SCOPEFAX = reader["SCOPEFAX"] != DBNull.Value ? reader["SCOPEFAX"].ToString() : null,
                                ASCAP1 = reader["ASCAP1"] != DBNull.Value ? (int?)reader["ASCAP1"] : null,
                                ASCAP2 = reader["ASCAP2"] != DBNull.Value ? (int?)reader["ASCAP2"] : null,
                                ASCAP3 = reader["ASCAP3"] != DBNull.Value ? (int?)reader["ASCAP3"] : null,
                                ASCAP4 = reader["ASCAP4"] != DBNull.Value ? (int?)reader["ASCAP4"] : null,
                                PEMAIL = reader["PEMAIL"] != DBNull.Value ? reader["PEMAIL"].ToString() : null,
                                PK = reader["PK"] != DBNull.Value && Convert.ToBoolean(reader["PK"]),
                                UPK = reader["UPK"] != DBNull.Value && Convert.ToBoolean(reader["UPK"]),
                                EADR21 = reader["EADR21"] != DBNull.Value ? reader["EADR21"].ToString() : null,
                                EADR22 = reader["EADR22"] != DBNull.Value ? reader["EADR22"].ToString() : null,
                                EADR23 = reader["EADR23"] != DBNull.Value ? reader["EADR23"].ToString() : null,
                                EPHONE2 = reader["EPHONE2"] != DBNull.Value ? reader["EPHONE2"].ToString() : null,
                                AMBPHONE = reader["AMBPHONE"] != DBNull.Value ? reader["AMBPHONE"].ToString() : null,
                                ADDEMGINFO = reader["ADDEMGINFO"] != DBNull.Value ? reader["ADDEMGINFO"].ToString() : null,
                                ROOM_NO2 = reader["ROOM_NO2"] != DBNull.Value ? reader["ROOM_NO2"].ToString() : null,
                                ROOM_NO3 = reader["ROOM_NO3"] != DBNull.Value ? reader["ROOM_NO3"].ToString() : null,
                                ROOM_NO4 = reader["ROOM_NO4"] != DBNull.Value ? reader["ROOM_NO4"].ToString() : null,
                                CAP1 = reader["CAP1"] != DBNull.Value ? (int?)reader["CAP1"] : null,
                                CAP2 = reader["CAP2"] != DBNull.Value ? (int?)reader["CAP2"] : null,
                                CAP3 = reader["CAP3"] != DBNull.Value ? (int?)reader["CAP3"] : null,
                                CAP4 = reader["CAP4"] != DBNull.Value ? (int?)reader["CAP4"] : null,
                                OSSPLACE = reader["OSSPLACE"] != DBNull.Value ? reader["OSSPLACE"].ToString() : null,
                                ScopeEmail = reader["ScopeEmail"] != DBNull.Value ? reader["ScopeEmail"].ToString() : null,
                                ScopePassword = reader["ScopePassword"] != DBNull.Value ? reader["ScopePassword"].ToString() : null,
                                MidPoint = reader["MidPoint"] != DBNull.Value ? reader["MidPoint"].ToString() : null,
                                StaffingAssistantName = reader["StaffingAssistantName"] != DBNull.Value ? reader["StaffingAssistantName"].ToString() : null,
                                StaffingAssistantPhone = reader["StaffingAssistantPhone"] != DBNull.Value ? reader["StaffingAssistantPhone"].ToString() : null,
                                StaffingAssistantFax = reader["StaffingAssistantFax"] != DBNull.Value ? reader["StaffingAssistantFax"].ToString() : null,
                                StaffingAssistantEmail = reader["StaffingAssistantEmail"] != DBNull.Value ? reader["StaffingAssistantEmail"].ToString() : null,
                                StaffingAssistantOthers = reader["StaffingAssistantOthers"] != DBNull.Value ? reader["StaffingAssistantOthers"].ToString() : null,
                                StaffingAssistantExt = reader["StaffingAssistantExt"] != DBNull.Value ? reader["StaffingAssistantExt"].ToString() : null,
                                StaffingAssistantForms = reader["StaffingAssistantForms"] != DBNull.Value ? reader["StaffingAssistantForms"].ToString() : null,
                                FoodContactName = reader["FoodContactName"] != DBNull.Value ? reader["FoodContactName"].ToString() : null,
                                FoodContactPhone = reader["FoodContactPhone"] != DBNull.Value ? reader["FoodContactPhone"].ToString() : null,
                                FoodContactFax = reader["FoodContactFax"] != DBNull.Value ? reader["FoodContactFax"].ToString() : null,
                                FoodContactEmail = reader["FoodContactEmail"] != DBNull.Value ? reader["FoodContactEmail"].ToString() : null,
                                FoodContactOthers = reader["FoodContactOthers"] != DBNull.Value ? reader["FoodContactOthers"].ToString() : null,
                                FoodContactExt = reader["FoodContactExt"] != DBNull.Value ? reader["FoodContactExt"].ToString() : null,
                                FoodContactForms = reader["FoodContactForms"] != DBNull.Value ? reader["FoodContactForms"].ToString() : null,
                                SupplyContactName = reader["SupplyContactName"] != DBNull.Value ? reader["SupplyContactName"].ToString() : null,
                                SupplyContactPhone = reader["SupplyContactPhone"] != DBNull.Value ? reader["SupplyContactPhone"].ToString() : null,
                                SupplyContactFax = reader["SupplyContactFax"] != DBNull.Value ? reader["SupplyContactFax"].ToString() : null,
                                SupplyContactEmail = reader["SupplyContactEmail"] != DBNull.Value ? reader["SupplyContactEmail"].ToString() : null,
                                SupplyContactOthers = reader["SupplyContactOthers"] != DBNull.Value ? reader["SupplyContactOthers"].ToString() : null,
                                SupplyContactExt = reader["SupplyContactExt"] != DBNull.Value ? reader["SupplyContactExt"].ToString() : null,
                                SupplyContactForms = reader["SupplyContactForms"] != DBNull.Value ? reader["SupplyContactForms"].ToString() : null,
                                PettyCashSpecialEventContactName = reader["PettyCashSpecialEventContactName"] != DBNull.Value ? reader["PettyCashSpecialEventContactName"].ToString() : null,
                                PettyCashSpecialEventContactPhone = reader["PettyCashSpecialEventContactPhone"] != DBNull.Value ? reader["PettyCashSpecialEventContactPhone"].ToString() : null,
                                PettyCashSpecialEventContactFax = reader["PettyCashSpecialEventContactFax"] != DBNull.Value ? reader["PettyCashSpecialEventContactFax"].ToString() : null,
                                PettyCashSpecialEventContactEmail = reader["PettyCashSpecialEventContactEmail"] != DBNull.Value ? reader["PettyCashSpecialEventContactEmail"].ToString() : null,
                                PettyCashSpecialEventContactOthers = reader["PettyCashSpecialEventContactOthers"] != DBNull.Value ? reader["PettyCashSpecialEventContactOthers"].ToString() : null,
                                PettyCashSpecialEventContactExt = reader["PettyCashSpecialEventContactExt"] != DBNull.Value ? reader["PettyCashSpecialEventContactExt"].ToString() : null,
                                PettyCashSpecialEventContactForms = reader["PettyCashSpecialEventContactForms"] != DBNull.Value ? reader["PettyCashSpecialEventContactForms"].ToString() : null,
                                FieldSupervisorName = reader["FieldSupervisorName"] != DBNull.Value ? reader["FieldSupervisorName"].ToString() : null,
                                FieldSupervisorPhone = reader["FieldSupervisorPhone"] != DBNull.Value ? reader["FieldSupervisorPhone"].ToString() : null,
                                FieldSupervisorFax = reader["FieldSupervisorFax"] != DBNull.Value ? reader["FieldSupervisorFax"].ToString() : null,
                                FieldSupervisorEmail = reader["FieldSupervisorEmail"] != DBNull.Value ? reader["FieldSupervisorEmail"].ToString() : null,
                                FieldSupervisorOthers = reader["FieldSupervisorOthers"] != DBNull.Value ? reader["FieldSupervisorOthers"].ToString() : null,
                                FieldSupervisorExt = reader["FieldSupervisorExt"] != DBNull.Value ? reader["FieldSupervisorExt"].ToString() : null,
                                FieldSupervisorForms = reader["FieldSupervisorForms"] != DBNull.Value ? reader["FieldSupervisorForms"].ToString() : null,
                                FieldTrainerName = reader["FieldTrainerName"] != DBNull.Value ? reader["FieldTrainerName"].ToString() : null,
                                FieldTrainerPhone = reader["FieldTrainerPhone"] != DBNull.Value ? reader["FieldTrainerPhone"].ToString() : null,
                                FieldTrainerFax = reader["FieldTrainerFax"] != DBNull.Value ? reader["FieldTrainerFax"].ToString() : null,
                                FieldTrainerEmail = reader["FieldTrainerEmail"] != DBNull.Value ? reader["FieldTrainerEmail"].ToString() : null,
                                FieldTrainerOthers = reader["FieldTrainerOthers"] != DBNull.Value ? reader["FieldTrainerOthers"].ToString() : null,
                                FieldTrainerExt = reader["FieldTrainerExt"] != DBNull.Value ? reader["FieldTrainerExt"].ToString() : null,
                                FieldTrainerForms = reader["FieldTrainerForms"] != DBNull.Value ? reader["FieldTrainerForms"].ToString() : null,
                                HealthCareConsultantName = reader["HealthCareConsultantName"] != DBNull.Value ? reader["HealthCareConsultantName"].ToString() : null,
                                HealthCareConsultantPhone = reader["HealthCareConsultantPhone"] != DBNull.Value ? reader["HealthCareConsultantPhone"].ToString() : null,
                                HealthCareConsultantFax = reader["HealthCareConsultantFax"] != DBNull.Value ? reader["HealthCareConsultantFax"].ToString() : null,
                                HealthCareConsultantEmail = reader["HealthCareConsultantEmail"] != DBNull.Value ? reader["HealthCareConsultantEmail"].ToString() : null,
                                HealthCareConsultantOthers = reader["HealthCareConsultantOthers"] != DBNull.Value ? reader["HealthCareConsultantOthers"].ToString() : null,
                                HealthCareConsultantExt = reader["HealthCareConsultantExt"] != DBNull.Value ? reader["HealthCareConsultantExt"].ToString() : null,
                                HealthCareConsultantForms = reader["HealthCareConsultantForms"] != DBNull.Value ? reader["HealthCareConsultantForms"].ToString() : null,
                                RegistarName = reader["RegistarName"] != DBNull.Value ? reader["RegistarName"].ToString() : null,
                                RegistarPhone = reader["RegistarPhone"] != DBNull.Value ? reader["RegistarPhone"].ToString() : null,
                                RegistarFax = reader["RegistarFax"] != DBNull.Value ? reader["RegistarFax"].ToString() : null,
                                RegistarEmail = reader["RegistarEmail"] != DBNull.Value ? reader["RegistarEmail"].ToString() : null,
                                RegistarOthers = reader["RegistarOthers"] != DBNull.Value ? reader["RegistarOthers"].ToString() : null,
                                RegistarExt = reader["RegistarExt"] != DBNull.Value ? reader["RegistarExt"].ToString() : null,
                                RegistarForms = reader["RegistarForms"] != DBNull.Value ? reader["RegistarForms"].ToString() : null,
                                AccountBillingName = reader["AccountBillingName"] != DBNull.Value ? reader["AccountBillingName"].ToString() : null,
                                AccountBillingPhone = reader["AccountBillingPhone"] != DBNull.Value ? reader["AccountBillingPhone"].ToString() : null,
                                AccountBillingFax = reader["AccountBillingFax"] != DBNull.Value ? reader["AccountBillingFax"].ToString() : null,
                                AccountBillingEmail = reader["AccountBillingEmail"] != DBNull.Value ? reader["AccountBillingEmail"].ToString() : null,
                                AccountBillingOthers = reader["AccountBillingOthers"] != DBNull.Value ? reader["AccountBillingOthers"].ToString() : null,
                                AccountBillingExt = reader["AccountBillingExt"] != DBNull.Value ? reader["AccountBillingExt"].ToString() : null,
                                AccountBillingForms = reader["AccountBillingForms"] != DBNull.Value ? reader["AccountBillingForms"].ToString() : null,
                                SubstitutesName = reader["SubstitutesName"] != DBNull.Value ? reader["SubstitutesName"].ToString() : null,
                                SubstitutesPhone = reader["SubstitutesPhone"] != DBNull.Value ? reader["SubstitutesPhone"].ToString() : null,
                                SubstitutesFax = reader["SubstitutesFax"] != DBNull.Value ? reader["SubstitutesFax"].ToString() : null,
                                SubstitutesEmail = reader["SubstitutesEmail"] != DBNull.Value ? reader["SubstitutesEmail"].ToString() : null,
                                SubstitutesOthers = reader["SubstitutesOthers"] != DBNull.Value ? reader["SubstitutesOthers"].ToString() : null,
                                SubstitutesExt = reader["SubstitutesExt"] != DBNull.Value ? reader["SubstitutesExt"].ToString() : null,
                                SubstitutesForms = reader["SubstitutesForms"] != DBNull.Value ? reader["SubstitutesForms"].ToString() : null,
                                SuppliesName = reader["SuppliesName"] != DBNull.Value ? reader["SuppliesName"].ToString() : null,
                                SuppliesPhone = reader["SuppliesPhone"] != DBNull.Value ? reader["SuppliesPhone"].ToString() : null,
                                SuppliesFax = reader["SuppliesFax"] != DBNull.Value ? reader["SuppliesFax"].ToString() : null,
                                SuppliesEmail = reader["SuppliesEmail"] != DBNull.Value ? reader["SuppliesEmail"].ToString() : null,
                                SuppliesOthers = reader["SuppliesOthers"] != DBNull.Value ? reader["SuppliesOthers"].ToString() : null,
                                SuppliesExt = reader["SuppliesExt"] != DBNull.Value ? reader["SuppliesExt"].ToString() : null,
                                SuppliesForms = reader["SuppliesForms"] != DBNull.Value ? reader["SuppliesForms"].ToString() : null,
                                AccountAssistantOthers = reader["AccountAssistantOthers"] != DBNull.Value ? reader["AccountAssistantOthers"].ToString() : null,
                                Type = reader["Type"] != DBNull.Value ? (long?)reader["Type"] : null,
                                Priority = reader["Priority"] != DBNull.Value ? (long?)reader["Priority"] : null,
                                SCOPEDSSName = reader["SCOPEDSSName"] != DBNull.Value ? reader["SCOPEDSSName"].ToString() : null,
                                SCOPEDSSPhone = reader["SCOPEDSSPhone"] != DBNull.Value ? reader["SCOPEDSSPhone"].ToString() : null,
                                SCOPEDSSFax = reader["SCOPEDSSFax"] != DBNull.Value ? reader["SCOPEDSSFax"].ToString() : null,
                                SCOPEDSSEmail = reader["SCOPEDSSEmail"] != DBNull.Value ? reader["SCOPEDSSEmail"].ToString() : null,
                                SCOPEDSSOthers = reader["SCOPEDSSOthers"] != DBNull.Value ? reader["SCOPEDSSOthers"].ToString() : null,
                                SCOPEDSSExt = reader["SCOPEDSSExt"] != DBNull.Value ? reader["SCOPEDSSExt"].ToString() : null,
                                SCOPEDSSForms = reader["SCOPEDSSForms"] != DBNull.Value ? reader["SCOPEDSSForms"].ToString() : null,
                                PresentersName = reader["PresentersName"] != DBNull.Value ? reader["PresentersName"].ToString() : null,
                                PresentersPhone = reader["PresentersPhone"] != DBNull.Value ? reader["PresentersPhone"].ToString() : null,
                                PresentersFax = reader["PresentersFax"] != DBNull.Value ? reader["PresentersFax"].ToString() : null,
                                PresentersEmail = reader["PresentersEmail"] != DBNull.Value ? reader["PresentersEmail"].ToString() : null,
                                PresentersOthers = reader["PresentersOthers"] != DBNull.Value ? reader["PresentersOthers"].ToString() : null,
                                PresentersExt = reader["PresentersExt"] != DBNull.Value ? reader["PresentersExt"].ToString() : null,
                                PresentersForms = reader["PresentersForms"] != DBNull.Value ? reader["PresentersForms"].ToString() : null,
                                AccountAssistantName = reader["AccountAssistantName"] != DBNull.Value ? reader["AccountAssistantName"].ToString() : null,
                                AccountAssistantPhone = reader["AccountAssistantPhone"] != DBNull.Value ? reader["AccountAssistantPhone"].ToString() : null,
                                AccountAssistantFax = reader["AccountAssistantFax"] != DBNull.Value ? reader["AccountAssistantFax"].ToString() : null,
                                AccountAssistantEmail = reader["AccountAssistantEmail"] != DBNull.Value ? reader["AccountAssistantEmail"].ToString() : null,
                            };

                            // Map to ExtendedSiteDto using AutoMapper
                            sites.Add(site);
                        }
                    }
                }
            }

            return sites;
        }

        public async Task<List<Site>> GetAllSitesByIdAsync(List<long?> siteIds)
        {
            return await _context.Sites
                   .Where(d => siteIds.Contains(d.SiteID))
                  .ToListAsync();
        }

        public async Task<List<Site>> GetAllSitesBySiteNumber(List<long?> siteNumber)
        {
            return await _context.Sites
                   .Where(d => siteNumber.Contains(d.SITE_NUM))
                  .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
