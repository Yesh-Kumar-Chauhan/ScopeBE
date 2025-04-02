using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_director_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    DirectorID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<long>(type: "bigint", nullable: false),
                    SiteID = table.Column<long>(type: "bigint", nullable: false),
                    MonAMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonAMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TueAMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TueAMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WedAMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WedAMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuAMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuAMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriAMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriAMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonPMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonPMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuePMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuePMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WedPMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WedPMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuPMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuPMTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriPMFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriPMTo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.DirectorID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Directors");
        }
    }
}
