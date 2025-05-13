using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferencesManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpeakerConferenceFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpeakerConferenceFile_SpeakerConference_SpeakerConferenceId",
                table: "SpeakerConferenceFile");

            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "SpeakerConferenceFile",
                newName: "UploadedAt");

            migrationBuilder.RenameColumn(
                name: "SpeakerConferenceId",
                table: "SpeakerConferenceFile",
                newName: "SpeakerId");

            migrationBuilder.RenameIndex(
                name: "IX_SpeakerConferenceFile_SpeakerConferenceId",
                table: "SpeakerConferenceFile",
                newName: "IX_SpeakerConferenceFile_SpeakerId");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "SpeakerConferenceFile",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerConferenceFile_ConferenceId",
                table: "SpeakerConferenceFile",
                column: "ConferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpeakerConferenceFile_Conferences_ConferenceId",
                table: "SpeakerConferenceFile",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpeakerConferenceFile_Delegates_SpeakerId",
                table: "SpeakerConferenceFile",
                column: "SpeakerId",
                principalTable: "Delegates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpeakerConferenceFile_Conferences_ConferenceId",
                table: "SpeakerConferenceFile");

            migrationBuilder.DropForeignKey(
                name: "FK_SpeakerConferenceFile_Delegates_SpeakerId",
                table: "SpeakerConferenceFile");

            migrationBuilder.DropIndex(
                name: "IX_SpeakerConferenceFile_ConferenceId",
                table: "SpeakerConferenceFile");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "SpeakerConferenceFile");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "SpeakerConferenceFile",
                newName: "UploadDate");

            migrationBuilder.RenameColumn(
                name: "SpeakerId",
                table: "SpeakerConferenceFile",
                newName: "SpeakerConferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_SpeakerConferenceFile_SpeakerId",
                table: "SpeakerConferenceFile",
                newName: "IX_SpeakerConferenceFile_SpeakerConferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpeakerConferenceFile_SpeakerConference_SpeakerConferenceId",
                table: "SpeakerConferenceFile",
                column: "SpeakerConferenceId",
                principalTable: "SpeakerConference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
