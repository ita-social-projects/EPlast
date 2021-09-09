using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RemoveColumnsIsCurrentDateFinishAndChangeHasManyToHasOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPlastDegrees_UserId",
                table: "UserPlastDegrees");

            migrationBuilder.DropColumn(
                name: "DateFinish",
                table: "UserPlastDegrees");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "UserPlastDegrees");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_UserId",
                table: "UserPlastDegrees",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPlastDegrees_UserId",
                table: "UserPlastDegrees");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFinish",
                table: "UserPlastDegrees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "UserPlastDegrees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_UserId",
                table: "UserPlastDegrees",
                column: "UserId");
        }
    }
}
