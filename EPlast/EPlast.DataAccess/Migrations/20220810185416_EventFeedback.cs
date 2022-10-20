using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class EventFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<double>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    ParticipantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventFeedbacks_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFeedbacks_ParticipantId",
                table: "EventFeedbacks",
                column: "ParticipantId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFeedbacks");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Events",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
