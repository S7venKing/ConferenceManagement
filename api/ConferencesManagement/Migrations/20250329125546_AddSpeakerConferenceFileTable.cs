using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferencesManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeakerConferenceFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeakerConferenceFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeakerConferenceId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerConferenceFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeakerConferenceFile_SpeakerConference_SpeakerConferenceId",
                        column: x => x.SpeakerConferenceId,
                        principalTable: "SpeakerConference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerConferenceFile_SpeakerConferenceId",
                table: "SpeakerConferenceFile",
                column: "SpeakerConferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeakerConferenceFile");
        }
    }
}
