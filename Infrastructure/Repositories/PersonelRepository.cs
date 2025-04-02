using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using EFCore.BulkExtensions;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PersonelRepository : IPersonelRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;


        public PersonelRepository(AppDbContext context, IConfiguration configuration, string connectionStringName = "ProdConnection")
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(connectionStringName);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public IQueryable<Personel> GetAllPersonel()
        {
            // Returns an IQueryable<Personel> for search and pagination
            return _context.Personel.AsQueryable();
        }

        public async Task<IEnumerable<Personel>> GetAllPersonelAsync()
        {
            // Asynchronously retrieves all personnel without filtering
            return await _context.Personel.ToListAsync();
        }

        public async Task<Personel?> GetPersonelByIdAsync(long id)
        {
            // Fetches a personnel by ID
            return await _context.Personel.FindAsync(id);
        }

        public async Task<Personel?> GetPersonelByEmailAsync(string email)
        {
            // Fetches a personnel by ID
            return await _context.Personel.FirstOrDefaultAsync(x => x.EMAIL == email);
        }


        public async Task<Personel> AddPersonelAsync(Personel personel)
        {
            // Adds a new personnel entity
            _context.Personel.Add(personel);
            await _context.SaveChangesAsync();
            return personel;
        }

        public async Task<Personel?> UpdatePersonelAsync(Personel personel)
        {
            // Updates an existing personnel entity
            var existingPersonel = await _context.Personel.FindAsync(personel.PersonalID);
            if (existingPersonel == null)
            {
                return null;
            }

            // Map the changes from the input personel to the existing entity
            _context.Entry(existingPersonel).CurrentValues.SetValues(personel);

            await _context.SaveChangesAsync();
            return existingPersonel;
        }

        public async Task<bool> DeletePersonelAsync(long id)
        {
            // Deletes a personnel entity by ID
            var personel = await _context.Personel.FindAsync(id);
            if (personel == null)
            {
                return false;
            }

            _context.Personel.Remove(personel);
            await _context.SaveChangesAsync();
            return true;
        }

        // Method for bulk updating personnel
        public async Task BulkUpdatePersonnelAsync(List<Personel> personnel)
        {
            await _context.BulkUpdateAsync(personnel, new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = false,
                // You can add more configuration here if needed
            });

            //_context.UpdateRange(personnel);
            //await _context.SaveChangesAsync();
        }

        public async Task<List<ExtendedPersonelDto>> GetPersonelByKeywordAndOperationAsync(string keyword, int operation)
        {
            var personels = new List<ExtendedPersonelDto>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_Personnel_Select", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@KeyWord", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(keyword) ? DBNull.Value : (object)keyword });
                    command.Parameters.Add(new SqlParameter("@Operation", SqlDbType.Int) { Value = operation });

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var personel = new ExtendedPersonelDto
                            {
                                // Mapping all properties from Personel entity
                                PersonalID = reader.GetInt64(reader.GetOrdinal("PersonalID")),
                                STAFF_ID = reader.IsDBNull(reader.GetOrdinal("STAFF_ID")) ? null : reader.GetInt64(reader.GetOrdinal("STAFF_ID")),
                                SSN = reader.IsDBNull(reader.GetOrdinal("SSN")) ? null : reader.GetString(reader.GetOrdinal("SSN")),
                                LASTNAME = reader.IsDBNull(reader.GetOrdinal("LASTNAME")) ? null : reader.GetString(reader.GetOrdinal("LASTNAME")),
                                FIRSTNAME = reader.IsDBNull(reader.GetOrdinal("FIRSTNAME")) ? null : reader.GetString(reader.GetOrdinal("FIRSTNAME")),
                                MI = reader.IsDBNull(reader.GetOrdinal("MI")) ? null : reader.GetString(reader.GetOrdinal("MI")),
                                STREET = reader.IsDBNull(reader.GetOrdinal("STREET")) ? null : reader.GetString(reader.GetOrdinal("STREET")),
                                CITY = reader.IsDBNull(reader.GetOrdinal("CITY")) ? null : reader.GetString(reader.GetOrdinal("CITY")),
                                STATE = reader.IsDBNull(reader.GetOrdinal("STATE")) ? null : reader.GetString(reader.GetOrdinal("STATE")),
                                ZIPCODE = reader.IsDBNull(reader.GetOrdinal("ZIPCODE")) ? null : reader.GetString(reader.GetOrdinal("ZIPCODE")),
                                HOMEPHONE = reader.IsDBNull(reader.GetOrdinal("HOMEPHONE")) ? null : reader.GetString(reader.GetOrdinal("HOMEPHONE")),
                                WORKPHONE = reader.IsDBNull(reader.GetOrdinal("WORKPHONE")) ? null : reader.GetString(reader.GetOrdinal("WORKPHONE")),
                                OTHERPHONE = reader.IsDBNull(reader.GetOrdinal("OTHERPHONE")) ? null : reader.GetString(reader.GetOrdinal("OTHERPHONE")),
                                DOEMP = reader.IsDBNull(reader.GetOrdinal("DOEMP")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DOEMP")),
                                DOTERM = reader.IsDBNull(reader.GetOrdinal("DOTERM")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DOTERM")),
                                DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DOB")),
                                WPREQ = reader.IsDBNull(reader.GetOrdinal("WPREQ")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("WPREQ")),
                                WPREC = reader.IsDBNull(reader.GetOrdinal("WPREC")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("WPREC")),
                                CLR2BMAIL = reader.IsDBNull(reader.GetOrdinal("CLR2BMAIL")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CLR2BMAIL")),
                                CLEARMAIL = reader.IsDBNull(reader.GetOrdinal("CLEARMAIL")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CLEARMAIL")),
                                CLEARREC = reader.IsDBNull(reader.GetOrdinal("CLEARREC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CLEARREC")),
                                APP = reader.IsDBNull(reader.GetOrdinal("APP")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("APP")),
                                W4 = reader.IsDBNull(reader.GetOrdinal("W4")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("W4")),
                                I9 = reader.IsDBNull(reader.GetOrdinal("I9")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("I9")),
                                MEDICALEXP = reader.IsDBNull(reader.GetOrdinal("MEDICALEXP")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MEDICALEXP")),
                                TINEEXP = reader.IsDBNull(reader.GetOrdinal("TINEEXP")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("TINEEXP")),
                                REF_P_OUT = reader.IsDBNull(reader.GetOrdinal("REF_P_OUT")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_P_OUT")),
                                REF_P_REC = reader.IsDBNull(reader.GetOrdinal("REF_P_REC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_P_REC")),
                                REF_W1_OUT = reader.IsDBNull(reader.GetOrdinal("REF_W1_OUT")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_W1_OUT")),
                                REF_W1_REC = reader.IsDBNull(reader.GetOrdinal("REF_W1_REC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_W1_REC")),
                                REF_W2_OUT = reader.IsDBNull(reader.GetOrdinal("REF_W2_OUT")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_W2_OUT")),
                                REF_W2_REC = reader.IsDBNull(reader.GetOrdinal("REF_W2_REC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_W2_REC")),
                                FINGERCNTY = reader.IsDBNull(reader.GetOrdinal("FINGERCNTY")) ? null : reader.GetString(reader.GetOrdinal("FINGERCNTY")),
                                SITE_NUM_B = reader.IsDBNull(reader.GetOrdinal("SITE_NUM_B")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("SITE_NUM_B")),
                                SITE_NAM_B = reader.IsDBNull(reader.GetOrdinal("SITE_NAM_B")) ? null : reader.GetString(reader.GetOrdinal("SITE_NAM_B")),
                                SITE_POS_B = reader.IsDBNull(reader.GetOrdinal("SITE_POS_B")) ? null : reader.GetString(reader.GetOrdinal("SITE_POS_B")),
                                SITE_NUM_D = reader.IsDBNull(reader.GetOrdinal("SITE_NUM_D")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("SITE_NUM_D")),
                                SITE_NAM_D = reader.IsDBNull(reader.GetOrdinal("SITE_NAM_D")) ? null : reader.GetString(reader.GetOrdinal("SITE_NAM_D")),
                                SITE_POS_D = reader.IsDBNull(reader.GetOrdinal("SITE_POS_D")) ? null : reader.GetString(reader.GetOrdinal("SITE_POS_D")),
                                SITE_NUM_A = reader.IsDBNull(reader.GetOrdinal("SITE_NUM_A")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("SITE_NUM_A")),
                                SITE_NAM_A = reader.IsDBNull(reader.GetOrdinal("SITE_NAM_A")) ? null : reader.GetString(reader.GetOrdinal("SITE_NAM_A")),
                                SITE_POS_A = reader.IsDBNull(reader.GetOrdinal("SITE_POS_A")) ? null : reader.GetString(reader.GetOrdinal("SITE_POS_A")),
                                MON_1_B = reader.IsDBNull(reader.GetOrdinal("MON_1_B")) ? null : reader.GetString(reader.GetOrdinal("MON_1_B")),
                                MON_1_E = reader.IsDBNull(reader.GetOrdinal("MON_1_E")) ? null : reader.GetString(reader.GetOrdinal("MON_1_E")),
                                MON_2_B = reader.IsDBNull(reader.GetOrdinal("MON_2_B")) ? null : reader.GetString(reader.GetOrdinal("MON_2_B")),
                                MON_2_E = reader.IsDBNull(reader.GetOrdinal("MON_2_E")) ? null : reader.GetString(reader.GetOrdinal("MON_2_E")),
                                MON_3_B = reader.IsDBNull(reader.GetOrdinal("MON_3_B")) ? null : reader.GetString(reader.GetOrdinal("MON_3_B")),
                                MON_3_E = reader.IsDBNull(reader.GetOrdinal("MON_3_E")) ? null : reader.GetString(reader.GetOrdinal("MON_3_E")),
                                TUE_1_B = reader.IsDBNull(reader.GetOrdinal("TUE_1_B")) ? null : reader.GetString(reader.GetOrdinal("TUE_1_B")),
                                TUE_1_E = reader.IsDBNull(reader.GetOrdinal("TUE_1_E")) ? null : reader.GetString(reader.GetOrdinal("TUE_1_E")),
                                TUE_2_B = reader.IsDBNull(reader.GetOrdinal("TUE_2_B")) ? null : reader.GetString(reader.GetOrdinal("TUE_2_B")),
                                TUE_2_E = reader.IsDBNull(reader.GetOrdinal("TUE_2_E")) ? null : reader.GetString(reader.GetOrdinal("TUE_2_E")),
                                TUE_3_B = reader.IsDBNull(reader.GetOrdinal("TUE_3_B")) ? null : reader.GetString(reader.GetOrdinal("TUE_3_B")),
                                TUE_3_E = reader.IsDBNull(reader.GetOrdinal("TUE_3_E")) ? null : reader.GetString(reader.GetOrdinal("TUE_3_E")),
                                WED_1_B = reader.IsDBNull(reader.GetOrdinal("WED_1_B")) ? null : reader.GetString(reader.GetOrdinal("WED_1_B")),
                                WED_1_E = reader.IsDBNull(reader.GetOrdinal("WED_1_E")) ? null : reader.GetString(reader.GetOrdinal("WED_1_E")),
                                WED_2_B = reader.IsDBNull(reader.GetOrdinal("WED_2_B")) ? null : reader.GetString(reader.GetOrdinal("WED_2_B")),
                                WED_2_E = reader.IsDBNull(reader.GetOrdinal("WED_2_E")) ? null : reader.GetString(reader.GetOrdinal("WED_2_E")),
                                WED_3_B = reader.IsDBNull(reader.GetOrdinal("WED_3_B")) ? null : reader.GetString(reader.GetOrdinal("WED_3_B")),
                                WED_3_E = reader.IsDBNull(reader.GetOrdinal("WED_3_E")) ? null : reader.GetString(reader.GetOrdinal("WED_3_E")),
                                THU_1_B = reader.IsDBNull(reader.GetOrdinal("THU_1_B")) ? null : reader.GetString(reader.GetOrdinal("THU_1_B")),
                                THU_1_E = reader.IsDBNull(reader.GetOrdinal("THU_1_E")) ? null : reader.GetString(reader.GetOrdinal("THU_1_E")),
                                THU_2_B = reader.IsDBNull(reader.GetOrdinal("THU_2_B")) ? null : reader.GetString(reader.GetOrdinal("THU_2_B")),
                                THU_2_E = reader.IsDBNull(reader.GetOrdinal("THU_2_E")) ? null : reader.GetString(reader.GetOrdinal("THU_2_E")),
                                THU_3_B = reader.IsDBNull(reader.GetOrdinal("THU_3_B")) ? null : reader.GetString(reader.GetOrdinal("THU_3_B")),
                                THU_3_E = reader.IsDBNull(reader.GetOrdinal("THU_3_E")) ? null : reader.GetString(reader.GetOrdinal("THU_3_E")),
                                FRI_1_B = reader.IsDBNull(reader.GetOrdinal("FRI_1_B")) ? null : reader.GetString(reader.GetOrdinal("FRI_1_B")),
                                FRI_1_E = reader.IsDBNull(reader.GetOrdinal("FRI_1_E")) ? null : reader.GetString(reader.GetOrdinal("FRI_1_E")),
                                FRI_2_B = reader.IsDBNull(reader.GetOrdinal("FRI_2_B")) ? null : reader.GetString(reader.GetOrdinal("FRI_2_B")),
                                FRI_2_E = reader.IsDBNull(reader.GetOrdinal("FRI_2_E")) ? null : reader.GetString(reader.GetOrdinal("FRI_2_E")),
                                FRI_3_B = reader.IsDBNull(reader.GetOrdinal("FRI_3_B")) ? null : reader.GetString(reader.GetOrdinal("FRI_3_B")),
                                FRI_3_E = reader.IsDBNull(reader.GetOrdinal("FRI_3_E")) ? null : reader.GetString(reader.GetOrdinal("FRI_3_E")),
                                PAY_RATE_B = reader.IsDBNull(reader.GetOrdinal("PAY_RATE_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PAY_RATE_B")),
                                SEP_PAY_RATE_B = reader.IsDBNull(reader.GetOrdinal("SEP_PAY_RATE_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SEP_PAY_RATE_B")),
                                JAN_PAY_RATE_B = reader.IsDBNull(reader.GetOrdinal("JAN_PAY_RATE_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("JAN_PAY_RATE_B")),
                                SALARY_B = reader.IsDBNull(reader.GetOrdinal("SALARY_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SALARY_B")),
                                MAX_HRS_B = reader.IsDBNull(reader.GetOrdinal("MAX_HRS_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MAX_HRS_B")),
                                PAY_RATE_D = reader.IsDBNull(reader.GetOrdinal("PAY_RATE_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PAY_RATE_D")),
                                SEP_PAY_RATE_D = reader.IsDBNull(reader.GetOrdinal("SEP_PAY_RATE_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SEP_PAY_RATE_D")),
                                JAN_PAY_RATE_D = reader.IsDBNull(reader.GetOrdinal("JAN_PAY_RATE_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("JAN_PAY_RATE_D")),
                                SALARY_D = reader.IsDBNull(reader.GetOrdinal("SALARY_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SALARY_D")),
                                MAX_HRS_D = reader.IsDBNull(reader.GetOrdinal("MAX_HRS_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MAX_HRS_D")),
                                PAY_RATE_A = reader.IsDBNull(reader.GetOrdinal("PAY_RATE_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PAY_RATE_A")),
                                SEP_PAY_RATE_A = reader.IsDBNull(reader.GetOrdinal("SEP_PAY_RATE_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SEP_PAY_RATE_A")),
                                JAN_PAY_RATE_A = reader.IsDBNull(reader.GetOrdinal("JAN_PAY_RATE_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("JAN_PAY_RATE_A")),
                                SALARY_A = reader.IsDBNull(reader.GetOrdinal("SALARY_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("SALARY_A")),
                                MAX_HRS_A = reader.IsDBNull(reader.GetOrdinal("MAX_HRS_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MAX_HRS_A")),
                                COMMENTS = reader.IsDBNull(reader.GetOrdinal("COMMENTS")) ? null : reader.GetString(reader.GetOrdinal("COMMENTS")),
                                IDCA = reader.IsDBNull(reader.GetOrdinal("IDCA")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IDCA")),
                                EDUCATION = reader.IsDBNull(reader.GetOrdinal("EDUCATION")) ? null : reader.GetString(reader.GetOrdinal("EDUCATION")),
                                DAYSOFF = reader.IsDBNull(reader.GetOrdinal("DAYSOFF")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DAYSOFF")),
                                ALLUSED = reader.IsDBNull(reader.GetOrdinal("ALLUSED")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("ALLUSED")),
                                DATESUSED = reader.IsDBNull(reader.GetOrdinal("DATESUSED")) ? null : reader.GetString(reader.GetOrdinal("DATESUSED")),
                                REFEREDBY = reader.IsDBNull(reader.GetOrdinal("REFEREDBY")) ? null : reader.GetString(reader.GetOrdinal("REFEREDBY")),
                                STATE_FPC = reader.IsDBNull(reader.GetOrdinal("STATE_FPC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("STATE_FPC")),
                                STATE_FPR = reader.IsDBNull(reader.GetOrdinal("STATE_FPR")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("STATE_FPR")),
                                HIRELETTER = reader.IsDBNull(reader.GetOrdinal("HIRELETTER")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("HIRELETTER")),
                                CNTY_FPC = reader.IsDBNull(reader.GetOrdinal("CNTY_FPC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CNTY_FPC")),
                                CNTY_FPR = reader.IsDBNull(reader.GetOrdinal("CNTY_FPR")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CNTY_FPR")),
                                REF_P_FON = reader.IsDBNull(reader.GetOrdinal("REF_P_FON")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_P_FON")),
                                REF_W1_FON = reader.IsDBNull(reader.GetOrdinal("REF_W1_FON")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_W1_FON")),
                                REF_W2_FON = reader.IsDBNull(reader.GetOrdinal("REF_W2_FON")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REF_W2_FON")),
                                ID1 = reader.IsDBNull(reader.GetOrdinal("ID1")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("ID1")),
                                ID2 = reader.IsDBNull(reader.GetOrdinal("ID2")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("ID2")),
                                MAX_ADD_B = reader.IsDBNull(reader.GetOrdinal("MAX_ADD_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MAX_ADD_B")),
                                MAX_ADD_D = reader.IsDBNull(reader.GetOrdinal("MAX_ADD_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MAX_ADD_D")),
                                MAX_ADD_A = reader.IsDBNull(reader.GetOrdinal("MAX_ADD_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MAX_ADD_A")),
                                DSS_POS = reader.IsDBNull(reader.GetOrdinal("DSS_POS")) ? null : reader.GetString(reader.GetOrdinal("DSS_POS")),
                                ABSENCES = reader.IsDBNull(reader.GetOrdinal("ABSENCES")) ? null : reader.GetString(reader.GetOrdinal("ABSENCES")),
                                NYSID = reader.IsDBNull(reader.GetOrdinal("NYSID")) ? null : reader.GetString(reader.GetOrdinal("NYSID")),
                                GENDER = reader.IsDBNull(reader.GetOrdinal("GENDER")) ? null : reader.GetString(reader.GetOrdinal("GENDER")),
                                DAYSUSED = reader.IsDBNull(reader.GetOrdinal("DAYSUSED")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DAYSUSED")),
                                PERC_B = reader.IsDBNull(reader.GetOrdinal("PERC_B")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PERC_B")),
                                PERC_D = reader.IsDBNull(reader.GetOrdinal("PERC_D")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PERC_D")),
                                PERC_A = reader.IsDBNull(reader.GetOrdinal("PERC_A")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("PERC_A")),
                                MATAPP = reader.IsDBNull(reader.GetOrdinal("MATAPP")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MATAPP")),
                                CPR = reader.IsDBNull(reader.GetOrdinal("CPR")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CPR")),
                                FIRSTAID = reader.IsDBNull(reader.GetOrdinal("FIRSTAID")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FIRSTAID")),
                                MATDATE = reader.IsDBNull(reader.GetOrdinal("MATDATE")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MATDATE")),
                                RAISEEFFB = reader.IsDBNull(reader.GetOrdinal("RAISEEFFB")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RAISEEFFB")),
                                RAISEEFFD = reader.IsDBNull(reader.GetOrdinal("RAISEEFFD")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RAISEEFFD")),
                                RAISEEFFA = reader.IsDBNull(reader.GetOrdinal("RAISEEFFA")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("RAISEEFFA")),
                                REHIREABLE = reader.IsDBNull(reader.GetOrdinal("REHIREABLE")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("REHIREABLE")),
                                COMMENT = reader.IsDBNull(reader.GetOrdinal("COMMENT")) ? null : reader.GetString(reader.GetOrdinal("COMMENT")),
                                EMAIL = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? null : reader.GetString(reader.GetOrdinal("EMAIL")),
                                EMERNAME1 = reader.IsDBNull(reader.GetOrdinal("EMERNAME1")) ? null : reader.GetString(reader.GetOrdinal("EMERNAME1")),
                                EMERPHONE1 = reader.IsDBNull(reader.GetOrdinal("EMERPHONE1")) ? null : reader.GetString(reader.GetOrdinal("EMERPHONE1")),
                                EMERCELL1 = reader.IsDBNull(reader.GetOrdinal("EMERCELL1")) ? null : reader.GetString(reader.GetOrdinal("EMERCELL1")),
                                EMERNAME2 = reader.IsDBNull(reader.GetOrdinal("EMERNAME2")) ? null : reader.GetString(reader.GetOrdinal("EMERNAME2")),
                                EMERPHONE2 = reader.IsDBNull(reader.GetOrdinal("EMERPHONE2")) ? null : reader.GetString(reader.GetOrdinal("EMERPHONE2")),
                                EMERCELL2 = reader.IsDBNull(reader.GetOrdinal("EMERCELL2")) ? null : reader.GetString(reader.GetOrdinal("EMERCELL2")),
                                DAYEMERG = reader.IsDBNull(reader.GetOrdinal("DAYEMERG")) ? null : reader.GetString(reader.GetOrdinal("DAYEMERG")),
                                REHIRED = reader.IsDBNull(reader.GetOrdinal("REHIRED")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("REHIRED")),
                                ALLOTTEDB = reader.IsDBNull(reader.GetOrdinal("ALLOTTEDB")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ALLOTTEDB")),
                                ALLOTTEDD = reader.IsDBNull(reader.GetOrdinal("ALLOTTEDD")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ALLOTTEDD")),
                                ALLOTTEDA = reader.IsDBNull(reader.GetOrdinal("ALLOTTEDA")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ALLOTTEDA")),
                                AIDEFOR1 = reader.IsDBNull(reader.GetOrdinal("AIDEFOR1")) ? null : reader.GetString(reader.GetOrdinal("AIDEFOR1")),
                                AIDEFOR2 = reader.IsDBNull(reader.GetOrdinal("AIDEFOR2")) ? null : reader.GetString(reader.GetOrdinal("AIDEFOR2")),
                                AIDEFOR3 = reader.IsDBNull(reader.GetOrdinal("AIDEFOR3")) ? null : reader.GetString(reader.GetOrdinal("AIDEFOR3")),
                                MatStart = reader.IsDBNull(reader.GetOrdinal("MatStart")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("MatStart")),
                                SEL = reader.IsDBNull(reader.GetOrdinal("SEL")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SEL")),
                                PersonalFieldSupervisor = reader.IsDBNull(reader.GetOrdinal("PersonalFieldSupervisor")) ? null : reader.GetString(reader.GetOrdinal("PersonalFieldSupervisor")),
                                PersonalFieldTrainer = reader.IsDBNull(reader.GetOrdinal("PersonalFieldTrainer")) ? null : reader.GetString(reader.GetOrdinal("PersonalFieldTrainer")),
                                PersonalHHC = reader.IsDBNull(reader.GetOrdinal("PersonalHHC")) ? null : reader.GetString(reader.GetOrdinal("PersonalHHC")),
                                Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Type")),
                                EffectiveDateBefore = reader.IsDBNull(reader.GetOrdinal("EffectiveDateBefore")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EffectiveDateBefore")),
                                EffectiveDateDuring = reader.IsDBNull(reader.GetOrdinal("EffectiveDateDuring")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EffectiveDateDuring")),
                                EffectiveDateAfter = reader.IsDBNull(reader.GetOrdinal("EffectiveDateAfter")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EffectiveDateAfter")),
                                Foundations = reader.IsDBNull(reader.GetOrdinal("Foundations")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Foundations")),
                                LeaveOfAbsense = reader.IsDBNull(reader.GetOrdinal("LeaveOfAbsense")) ? false : reader.GetBoolean(reader.GetOrdinal("LeaveOfAbsense")),
                                LeaveStartDate = reader.IsDBNull(reader.GetOrdinal("LeaveStartDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LeaveStartDate")),
                                LeaveEndDate = reader.IsDBNull(reader.GetOrdinal("LeaveEndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LeaveEndDate")),
                                SuspensionStartDate = reader.IsDBNull(reader.GetOrdinal("SuspensionStartDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SuspensionStartDate")),
                                SuspensionEndDate = reader.IsDBNull(reader.GetOrdinal("SuspensionEndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SuspensionEndDate")),
                                Leaves = reader.IsDBNull(reader.GetOrdinal("Leaves")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Leaves")),
                                LeavesStart = reader.IsDBNull(reader.GetOrdinal("LeavesStart")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LeavesStart")),
                                LeavesEnd = reader.IsDBNull(reader.GetOrdinal("LeavesEnd")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LeavesEnd")),
                                Suspension = reader.IsDBNull(reader.GetOrdinal("Suspension")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Suspension")),
                                SuspensionStart = reader.IsDBNull(reader.GetOrdinal("SuspensionStart")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SuspensionStart")),
                                SuspensionEnd = reader.IsDBNull(reader.GetOrdinal("SuspensionEnd")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SuspensionEnd")),
                                ExpungeDate = reader.IsDBNull(reader.GetOrdinal("ExpungeDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ExpungeDate")),
                                AloneWithChildren = reader.IsDBNull(reader.GetOrdinal("AloneWithChildren")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("AloneWithChildren")),
                                FlagType = reader.IsDBNull(reader.GetOrdinal("FlagType")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagType")),
                                SHarassmentExp = reader.IsDBNull(reader.GetOrdinal("SHarassmentExp")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SHarassmentExp")),
                                SHarassmentExp2 = reader.IsDBNull(reader.GetOrdinal("SHarassmentExp2")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SHarassmentExp2")),
                                CBC = reader.IsDBNull(reader.GetOrdinal("CBC")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CBC")),
                                ACES = reader.IsDBNull(reader.GetOrdinal("ACES")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ACES")),
                                ELaw = reader.IsDBNull(reader.GetOrdinal("ELaw")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ELaw")),
                                FingerprintDate = reader.IsDBNull(reader.GetOrdinal("FingerprintDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FingerprintDate")),
                                Foundations15H = reader.IsDBNull(reader.GetOrdinal("Foundations15H")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("Foundations15H")),
                            };

                            // Set additional properties manually
                            personel.FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? null : reader.GetString(reader.GetOrdinal("FullName"));
                            if (operation != 2 && operation != 1) // Example: only for specific operations
                            {
                                personel.Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Total"));
                                personel.Active = reader.IsDBNull(reader.GetOrdinal("Active")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Active"));
                                personel.Terminated = reader.IsDBNull(reader.GetOrdinal("Terminated")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Terminated"));
                            }

                            personels.Add(personel);
                        }
                    }
                }
            }

            return personels;
        }

        public async Task<List<Personel>> GetPersonelsByIdsAsync(List<long?> personnelIds)
        {
            return await _context.Personel
                   .Where(d => personnelIds.Contains(d.PersonalID))
                  .ToListAsync();
        }
    }
}
