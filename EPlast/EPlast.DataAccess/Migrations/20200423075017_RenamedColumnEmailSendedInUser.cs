using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RenamedColumnEmailSendedInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailSended",
                table: "AspNetUsers",
                newName: "EmailSendedAfterRegistration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailSendedAfterRegistration",
                table: "AspNetUsers",
                newName: "EmailSended");
        }
    }
}
