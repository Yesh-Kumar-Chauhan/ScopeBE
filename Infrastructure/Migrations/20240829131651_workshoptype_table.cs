using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class workshoptype_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inservices",
                columns: table => new
                {
                    InserviceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STAFF_ID = table.Column<long>(type: "bigint", nullable: true),
                    TRAINING = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HOURS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TopicID = table.Column<long>(type: "bigint", nullable: true),
                    SPONSOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkShopTypeID = table.Column<long>(type: "bigint", nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inservices", x => x.InserviceID);
                });

            migrationBuilder.CreateTable(
                name: "TopicType",
                columns: table => new
                {
                    TopicID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicType", x => x.TopicID);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopType",
                columns: table => new
                {
                    WorkshopTypeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkshopTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopType", x => x.WorkshopTypeID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inservices");

            migrationBuilder.DropTable(
                name: "TopicType");

            migrationBuilder.DropTable(
                name: "WorkshopType");
        }
    }
}
