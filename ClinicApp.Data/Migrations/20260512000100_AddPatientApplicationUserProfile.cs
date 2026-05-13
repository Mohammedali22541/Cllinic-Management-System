using ClinicApp.Data.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicApp.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ClinicManagementDbContext))]
    [Migration("20260512000100_AddPatientApplicationUserProfile")]
    public partial class AddPatientApplicationUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_ApplicationUserId",
                table: "Patients",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_ApplicationUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ApplicationUserId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Patients");
        }
    }
}