using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class attendance_table_site : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    AttendanceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STAFF_ID = table.Column<long>(type: "bigint", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REASON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAID = table.Column<bool>(type: "bit", nullable: true),
                    CHARGED = table.Column<bool>(type: "bit", nullable: true),
                    FRACTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SITENUM = table.Column<long>(type: "bigint", nullable: true),
                    SITENAM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.AttendanceID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance");
        }
    }
}
