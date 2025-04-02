using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upate_schedule_timesheet_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchedularTimesheets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    PersonID = table.Column<long>(type: "bigint", nullable: true),
                    SiteID = table.Column<long>(type: "bigint", nullable: true),
                    SiteType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    LunchIn = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchOut = table.Column<TimeSpan>(type: "time", nullable: true),
                    AdditionalStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    AdditionalStop = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedularTimesheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedularTimesheets_Personel_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Personel",
                        principalColumn: "PersonalID");
                    table.ForeignKey(
                        name: "FK_SchedularTimesheets_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SchedularTimesheets_Sites_SiteID",
                        column: x => x.SiteID,
                        principalTable: "Sites",
                        principalColumn: "SiteID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedularTimesheets_PersonID",
                table: "SchedularTimesheets",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_SchedularTimesheets_ScheduleId",
                table: "SchedularTimesheets",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedularTimesheets_SiteID",
                table: "SchedularTimesheets",
                column: "SiteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedularTimesheets");
        }
    }
}
