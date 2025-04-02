using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class contact_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteID = table.Column<long>(type: "bigint", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CONTACT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CHILD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SITUATION = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
