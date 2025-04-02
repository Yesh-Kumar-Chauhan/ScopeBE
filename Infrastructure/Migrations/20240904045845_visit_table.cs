using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class visit_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    VisitID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteID = table.Column<long>(type: "bigint", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIMEIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIMEOUT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OFFICAL = table.Column<bool>(type: "bit", nullable: true),
                    STAFFING = table.Column<bool>(type: "bit", nullable: true),
                    PROBLEM = table.Column<bool>(type: "bit", nullable: true),
                    TRAINING = table.Column<bool>(type: "bit", nullable: true),
                    QUALITY = table.Column<bool>(type: "bit", nullable: true),
                    OTHER = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.VisitID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visits");
        }
    }
}
