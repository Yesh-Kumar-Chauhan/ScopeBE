using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_WavierReceived_table_site : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaiversReceived",
                columns: table => new
                {
                    WaiversReceivedID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STAFF_ID = table.Column<long>(type: "bigint", nullable: true),
                    SIte_ID = table.Column<long>(type: "bigint", nullable: true),
                    RECEIVED = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaiversReceived", x => x.WaiversReceivedID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaiversReceived");
        }
    }
}
