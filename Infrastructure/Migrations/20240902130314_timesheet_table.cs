using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class timesheet_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Timesheet",
                columns: table => new
                {
                    TimesheetID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictID = table.Column<long>(type: "bigint", nullable: true),
                    SiteID = table.Column<long>(type: "bigint", nullable: true),
                    SchoolID = table.Column<long>(type: "bigint", nullable: true),
                    PersonID = table.Column<long>(type: "bigint", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeSheetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeIn = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeOut = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchIn = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchOut = table.Column<TimeSpan>(type: "time", nullable: true),
                    AdditionalStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    AdditionalStop = table.Column<TimeSpan>(type: "time", nullable: true),
                    DeviceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClockInLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClockOutLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotesHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheet", x => x.TimesheetID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timesheet");
        }
    }
}
