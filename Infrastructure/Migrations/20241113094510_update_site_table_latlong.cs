using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_site_table_latlong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Schools");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Sites",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Sites",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Sites");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Schools",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Schools",
                type: "float",
                nullable: true);
        }
    }
}
