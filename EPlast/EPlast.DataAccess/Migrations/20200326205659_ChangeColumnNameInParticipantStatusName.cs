using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangeColumnNameInParticipantStatusName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEventStatusName",
                table: "ParticipantStatuses",
                newName: "ParticipantStatusName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParticipantStatusName",
                table: "ParticipantStatuses",
                newName: "UserEventStatusName");
        }
    }
}
