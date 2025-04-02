using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_schedule_table_district_col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistName",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DistNumber",
                table: "Schedules",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistName",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DistNumber",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "Schedules");
        }
    }
}
