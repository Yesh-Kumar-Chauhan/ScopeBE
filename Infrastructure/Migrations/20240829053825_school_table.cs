using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class school_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    SchoolID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCH_NUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SCH_NAM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRINCIPAL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDR2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIST_NUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIST_NAM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SITE_NUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SITE_NAM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DISMISAL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TRANS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hidden = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.SchoolID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
