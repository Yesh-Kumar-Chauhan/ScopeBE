using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class schedule_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<long>(type: "bigint", nullable: true),
                    SiteType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    LunchIn = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchOut = table.Column<TimeSpan>(type: "time", nullable: true),
                    AdditionalStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    AdditionalStop = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Personel_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Personel",
                        principalColumn: "PersonalID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PersonID",
                table: "Schedules",
                column: "PersonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
