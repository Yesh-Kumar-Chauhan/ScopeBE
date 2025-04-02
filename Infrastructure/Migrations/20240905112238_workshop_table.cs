using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class workshop_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    WorkshopID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Sponsor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TypeID = table.Column<long>(type: "bigint", nullable: true),
                    WorkshopName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.WorkshopID);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopMembers",
                columns: table => new
                {
                    WorkshopMemberID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopID = table.Column<long>(type: "bigint", nullable: true),
                    PersonID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopMembers", x => x.WorkshopMemberID);
                    table.ForeignKey(
                        name: "FK_WorkshopMembers_Workshops_WorkshopID",
                        column: x => x.WorkshopID,
                        principalTable: "Workshops",
                        principalColumn: "WorkshopID");
                });

            migrationBuilder.CreateTable(
                name: "WorkshopTopics",
                columns: table => new
                {
                    WorkshopTopicID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopID = table.Column<long>(type: "bigint", nullable: true),
                    TopicID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopTopics", x => x.WorkshopTopicID);
                    table.ForeignKey(
                        name: "FK_WorkshopTopics_Workshops_WorkshopID",
                        column: x => x.WorkshopID,
                        principalTable: "Workshops",
                        principalColumn: "WorkshopID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopMembers_WorkshopID",
                table: "WorkshopMembers",
                column: "WorkshopID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopTopics_WorkshopID",
                table: "WorkshopTopics",
                column: "WorkshopID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkshopMembers");

            migrationBuilder.DropTable(
                name: "WorkshopTopics");

            migrationBuilder.DropTable(
                name: "Workshops");
        }
    }
}
