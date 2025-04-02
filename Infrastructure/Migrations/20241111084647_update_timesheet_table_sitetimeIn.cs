using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_timesheet_table_sitetimeIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExternalEventId",
                table: "Timesheet",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SiteTimeIn",
                table: "Timesheet",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SiteTimeOut",
                table: "Timesheet",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalEventId",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "SiteTimeIn",
                table: "Timesheet");

            migrationBuilder.DropColumn(
                name: "SiteTimeOut",
                table: "Timesheet");
        }
    }
}
