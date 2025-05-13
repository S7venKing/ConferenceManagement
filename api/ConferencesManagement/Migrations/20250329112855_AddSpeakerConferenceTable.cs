using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferencesManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeakerConferenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeakerConference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpeakerId = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeakerConference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeakerConference_Conferences_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeakerConference_Delegates_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Delegates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerConference_ConferenceId",
                table: "SpeakerConference",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeakerConference_SpeakerId",
                table: "SpeakerConference",
                column: "SpeakerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeakerConference");
        }
    }
}
