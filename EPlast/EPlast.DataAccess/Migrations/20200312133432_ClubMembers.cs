using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ClubMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                table: "ClubAdministrations");

            migrationBuilder.AlterColumn<int>(
                name: "ClubMembersID",
                table: "ClubAdministrations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                table: "ClubAdministrations",
                column: "ClubMembersID",
                principalTable: "ClubMembers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                table: "ClubAdministrations");

            migrationBuilder.AlterColumn<int>(
                name: "ClubMembersID",
                table: "ClubAdministrations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                table: "ClubAdministrations",
                column: "ClubMembersID",
                principalTable: "ClubMembers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
