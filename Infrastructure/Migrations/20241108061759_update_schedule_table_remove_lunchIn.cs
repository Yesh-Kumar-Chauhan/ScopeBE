using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_schedule_table_remove_lunchIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalStart",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "AdditionalStop",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LunchIn",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LunchOut",
                table: "Schedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AdditionalStart",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AdditionalStop",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LunchIn",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LunchOut",
                table: "Schedules",
                type: "time",
                nullable: true);
        }
    }
}
