using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DIST_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DIST_NAM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LIAISON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TITLE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SECRETARY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LPHONE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LFAX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LIASON2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TITLE2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SECRETARY2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR22 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR32 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LPHONE2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LPAX2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUPER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STREET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PHONE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FAX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONTRACT = table.Column<bool>(type: "bit", nullable: true),
                    TERMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KNDRGRTN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACTIVE = table.Column<bool>(type: "bit", nullable: true),
                    CLASS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COUNTY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KINREG = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KINPER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KINFON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RSPNSBL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAIL1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAIL2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LEMAIL1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LEMAIL2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BHEMERCON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BHEMERFON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BUILDING = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAILSUPER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUPERVISOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRAINER = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.DistrictID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
