using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddColumnsToOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "UpuDegreeID",
                table: "UserProfiles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                table: "Organization",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Organization",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Organization",
                maxLength: 18,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                table: "UserProfiles",
                column: "UpuDegreeID",
                principalTable: "UpuDegrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Organization");

            migrationBuilder.AlterColumn<int>(
                name: "UpuDegreeID",
                table: "UserProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                table: "UserProfiles",
                column: "UpuDegreeID",
                principalTable: "UpuDegrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
