using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upate_schedule_table_site : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<long>(
                name: "SiteID",
                table: "Schedules",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteType",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SiteID",
                table: "Schedules",
                column: "SiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Sites_SiteID",
                table: "Schedules",
                column: "SiteID",
                principalTable: "Sites",
                principalColumn: "SiteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Sites_SiteID",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_SiteID",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SiteID",
                table: "Schedules");

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
    }
}
