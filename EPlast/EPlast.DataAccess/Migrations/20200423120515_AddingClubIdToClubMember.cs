using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddingClubIdToClubMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_Clubs_ClubID",
                table: "ClubMembers");

            migrationBuilder.RenameColumn(
                name: "ClubID",
                table: "ClubMembers",
                newName: "ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_ClubMembers_ClubID",
                table: "ClubMembers",
                newName: "IX_ClubMembers_ClubId");

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "ClubMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FatherName",
                table: "AspNetUsers",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_Clubs_ClubId",
                table: "ClubMembers",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_Clubs_ClubId",
                table: "ClubMembers");

            migrationBuilder.RenameColumn(
                name: "ClubId",
                table: "ClubMembers",
                newName: "ClubID");

            migrationBuilder.RenameIndex(
                name: "IX_ClubMembers_ClubId",
                table: "ClubMembers",
                newName: "IX_ClubMembers_ClubID");

            migrationBuilder.AlterColumn<int>(
                name: "ClubID",
                table: "ClubMembers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FatherName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_Clubs_ClubID",
                table: "ClubMembers",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
