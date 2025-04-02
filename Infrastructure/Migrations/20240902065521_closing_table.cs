using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class closing_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Closings",
                columns: table => new
                {
                    ClosingID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictID = table.Column<long>(type: "bigint", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<long>(type: "bigint", nullable: true),
                    PARENT_CR = table.Column<bool>(type: "bit", nullable: false),
                    STAFF_PH = table.Column<bool>(type: "bit", nullable: false),
                    STAFF_DT = table.Column<bool>(type: "bit", nullable: false),
                    STAFF_ALL = table.Column<bool>(type: "bit", nullable: false),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MakeUpDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Closings", x => x.ClosingID);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Closings");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
