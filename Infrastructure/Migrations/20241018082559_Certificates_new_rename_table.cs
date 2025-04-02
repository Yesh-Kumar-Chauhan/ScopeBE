using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Certificates_new_rename_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    CertificateID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateTypeID = table.Column<long>(type: "bigint", nullable: true),
                    PersonID = table.Column<long>(type: "bigint", nullable: true),
                    CertificatePermanent = table.Column<bool>(type: "bit", nullable: true),
                    CertificateProfessional = table.Column<bool>(type: "bit", nullable: true),
                    CertificateCQ = table.Column<bool>(type: "bit", nullable: true),
                    Initial = table.Column<bool>(type: "bit", nullable: true),
                    InitialExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Provisional = table.Column<bool>(type: "bit", nullable: true),
                    ProvisionalExpiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.CertificateID);
                });

            migrationBuilder.CreateTable(
                name: "CertificateType",
                columns: table => new
                {
                    CertificateTypeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateType", x => x.CertificateTypeID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "CertificateType");
        }
    }
}
