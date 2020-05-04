using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class changingclubadmins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_AdminTypes_AdminTypeID",
                table: "ClubAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubID",
                table: "ClubAdministrations");

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

            migrationBuilder.RenameColumn(
                name: "ClubID",
                table: "ClubAdministrations",
                newName: "ClubId");

            migrationBuilder.RenameColumn(
                name: "AdminTypeID",
                table: "ClubAdministrations",
                newName: "AdminTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ClubAdministrations_ClubID",
                table: "ClubAdministrations",
                newName: "IX_ClubAdministrations_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_ClubAdministrations_AdminTypeID",
                table: "ClubAdministrations",
                newName: "IX_ClubAdministrations_AdminTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "ClubMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminTypeId",
                table: "ClubAdministrations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_AdminTypes_AdminTypeId",
                table: "ClubAdministrations",
                column: "AdminTypeId",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubId",
                table: "ClubAdministrations",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_ClubAdministrations_AdminTypes_AdminTypeId",
                table: "ClubAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubId",
                table: "ClubAdministrations");

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

            migrationBuilder.RenameColumn(
                name: "ClubId",
                table: "ClubAdministrations",
                newName: "ClubID");

            migrationBuilder.RenameColumn(
                name: "AdminTypeId",
                table: "ClubAdministrations",
                newName: "AdminTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_ClubAdministrations_ClubId",
                table: "ClubAdministrations",
                newName: "IX_ClubAdministrations_ClubID");

            migrationBuilder.RenameIndex(
                name: "IX_ClubAdministrations_AdminTypeId",
                table: "ClubAdministrations",
                newName: "IX_ClubAdministrations_AdminTypeID");

            migrationBuilder.AlterColumn<int>(
                name: "ClubID",
                table: "ClubMembers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AdminTypeID",
                table: "ClubAdministrations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_AdminTypes_AdminTypeID",
                table: "ClubAdministrations",
                column: "AdminTypeID",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubID",
                table: "ClubAdministrations",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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
