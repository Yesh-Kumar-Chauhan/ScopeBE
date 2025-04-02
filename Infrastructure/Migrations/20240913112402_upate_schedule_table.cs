using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upate_schedule_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteType",
                table: "Schedules");

            migrationBuilder.AddColumn<bool>(
                name: "SiteTypeAfter",
                table: "Schedules",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SiteTypeBefore",
                table: "Schedules",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SiteTypeDuring",
                table: "Schedules",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteTypeAfter",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SiteTypeBefore",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SiteTypeDuring",
                table: "Schedules");

            migrationBuilder.AddColumn<string>(
                name: "SiteType",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
