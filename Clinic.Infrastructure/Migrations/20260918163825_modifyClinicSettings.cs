using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifyClinicSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClinicSettings",
                columns: new[] { "Id", "Address", "ClinicName", "CreatedAt", "DeletedAt", "Description", "DoctorName", "Email", "FacebookUrl", "HeroImageUrl", "InstagramUrl", "IsDeleted", "LogoUrl", "Specialization", "UpdatedAt", "WhatsAppNumber" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Clinic Address", "Clinic Name", new DateTime(2026, 9, 18, 16, 38, 22, 826, DateTimeKind.Utc).AddTicks(1280), null, null, "Doctor Name", null, null, null, null, false, null, null, null, null });

            migrationBuilder.AddCheckConstraint(
                name: "CK_ClinicSettings_SingleRow",
                table: "ClinicSettings",
                sql: "[Id] = '11111111-1111-1111-1111-111111111111'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ClinicSettings_SingleRow",
                table: "ClinicSettings");

            migrationBuilder.DeleteData(
                table: "ClinicSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
