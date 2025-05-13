using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferencesManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notificationss",
                table: "Notificationss");

            migrationBuilder.RenameTable(
                name: "Notificationss",
                newName: "Notifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notificationss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notificationss",
                table: "Notificationss",
                column: "Id");
        }
    }
}
