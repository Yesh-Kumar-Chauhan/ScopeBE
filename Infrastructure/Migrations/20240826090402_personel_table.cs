using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class personel_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personel",
                columns: table => new
                {
                    PersonalID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STAFF_ID = table.Column<long>(type: "bigint", nullable: true),
                    SSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LASTNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FIRSTNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STREET = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CITY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZIPCODE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HOMEPHONE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WORKPHONE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OTHERPHONE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOEMP = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOTERM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WPREQ = table.Column<bool>(type: "bit", nullable: true),
                    WPREC = table.Column<bool>(type: "bit", nullable: true),
                    CLR2BMAIL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CLEARMAIL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CLEARREC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    APP = table.Column<bool>(type: "bit", nullable: true),
                    W4 = table.Column<bool>(type: "bit", nullable: true),
                    I9 = table.Column<bool>(type: "bit", nullable: true),
                    MEDICALEXP = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TINEEXP = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_P_OUT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_P_REC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_W1_OUT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_W1_REC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_W2_OUT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_W2_REC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FINGERCNTY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SITE_NUM_B = table.Column<long>(type: "bigint", nullable: true),
                    SITE_NAM_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SITE_POS_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SITE_NUM_D = table.Column<long>(type: "bigint", nullable: true),
                    SITE_NAM_D = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SITE_POS_D = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SITE_NUM_A = table.Column<long>(type: "bigint", nullable: true),
                    SITE_NAM_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SITE_POS_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MON_1_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MON_1_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MON_2_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MON_2_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MON_3_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MON_3_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TUE_1_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TUE_1_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TUE_2_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TUE_2_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TUE_3_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TUE_3_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WED_1_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WED_1_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WED_2_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WED_2_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WED_3_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WED_3_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    THU_1_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    THU_1_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    THU_2_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    THU_2_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    THU_3_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    THU_3_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRI_1_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRI_1_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRI_2_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRI_2_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRI_3_B = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRI_3_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PAY_RATE_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SEP_PAY_RATE_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JAN_PAY_RATE_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SALARY_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MAX_HRS_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PAY_RATE_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SEP_PAY_RATE_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JAN_PAY_RATE_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SALARY_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MAX_HRS_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PAY_RATE_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SEP_PAY_RATE_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    JAN_PAY_RATE_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SALARY_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MAX_HRS_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    COMMENTS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDCA = table.Column<bool>(type: "bit", nullable: true),
                    EDUCATION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DAYSOFF = table.Column<int>(type: "int", nullable: true),
                    ALLUSED = table.Column<bool>(type: "bit", nullable: true),
                    DATESUSED = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REFEREDBY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATE_FPC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATE_FPR = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HIRELETTER = table.Column<bool>(type: "bit", nullable: true),
                    CNTY_FPC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CNTY_FPR = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_P_FON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_W1_FON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REF_W2_FON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ID1 = table.Column<bool>(type: "bit", nullable: true),
                    ID2 = table.Column<bool>(type: "bit", nullable: true),
                    MAX_ADD_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MAX_ADD_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MAX_ADD_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DSS_POS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ABSENCES = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NYSID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GENDER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DAYSUSED = table.Column<int>(type: "int", nullable: true),
                    PERC_B = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PERC_D = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PERC_A = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MATAPP = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CPR = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FIRSTAID = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MATDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RAISEEFFB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RAISEEFFD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RAISEEFFA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REHIREABLE = table.Column<bool>(type: "bit", nullable: true),
                    COMMENT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMERNAME1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMERPHONE1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMERCELL1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMERNAME2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMERPHONE2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMERCELL2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DAYEMERG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REHIRED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ALLOTTEDB = table.Column<int>(type: "int", nullable: true),
                    ALLOTTEDD = table.Column<int>(type: "int", nullable: true),
                    ALLOTTEDA = table.Column<int>(type: "int", nullable: true),
                    AIDEFOR1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AIDEFOR2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AIDEFOR3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SEL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonalFieldSupervisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalFieldTrainer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalHHC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    EffectiveDateBefore = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveDateDuring = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveDateAfter = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Foundations = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaveOfAbsense = table.Column<bool>(type: "bit", nullable: false),
                    LeaveStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaveEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuspensionStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuspensionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Leaves = table.Column<bool>(type: "bit", nullable: true),
                    LeavesStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeavesEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Suspension = table.Column<bool>(type: "bit", nullable: true),
                    SuspensionStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuspensionEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpungeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AloneWithChildren = table.Column<bool>(type: "bit", nullable: true),
                    FlagType = table.Column<bool>(type: "bit", nullable: true),
                    SHarassmentExp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SHarassmentExp2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CBC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ACES = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ELaw = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FingerprintDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Foundations15H = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personel", x => x.PersonalID);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personel");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
