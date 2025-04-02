using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_schedule_table_remove_timeIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeOut",
                table: "Schedules",
                newName: "WednesdayTimeOut");

            migrationBuilder.RenameColumn(
                name: "TimeIn",
                table: "Schedules",
                newName: "WednesdayTimeIn");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayTimeIn",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "FridayTimeOut",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayTimeIn",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MondayTimeOut",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayTimeIn",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ThursdayTimeOut",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayTimeIn",
                table: "Schedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TuesdayTimeOut",
                table: "Schedules",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FridayTimeIn",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "FridayTimeOut",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "MondayTimeIn",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "MondayTimeOut",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ThursdayTimeIn",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ThursdayTimeOut",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TuesdayTimeIn",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TuesdayTimeOut",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "WednesdayTimeOut",
                table: "Schedules",
                newName: "TimeOut");

            migrationBuilder.RenameColumn(
                name: "WednesdayTimeIn",
                table: "Schedules",
                newName: "TimeIn");
        }
    }
}
