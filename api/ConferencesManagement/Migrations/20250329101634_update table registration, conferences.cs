using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferencesManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatetableregistrationconferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedRegistrationFee",
                table: "Conferences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "RegistrationFee",
                table: "Conferences",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "NeedRegistrationFee",
                table: "ConferenceHostingRegistration",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "RegistrationFee",
                table: "ConferenceHostingRegistration",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedRegistrationFee",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "RegistrationFee",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "NeedRegistrationFee",
                table: "ConferenceHostingRegistration");

            migrationBuilder.DropColumn(
                name: "RegistrationFee",
                table: "ConferenceHostingRegistration");
        }
    }
}
