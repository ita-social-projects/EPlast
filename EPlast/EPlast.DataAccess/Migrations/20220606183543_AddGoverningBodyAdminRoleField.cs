using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddGoverningBodyAdminRoleField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations");

            migrationBuilder.AlterColumn<int>(
                name: "GoverningBodyId",
                table: "GoverningBodyAdministrations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "GoverningBodyAdminRole",
                table: "GoverningBodyAdministrations",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations",
                column: "GoverningBodyId",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations");

            migrationBuilder.DropColumn(
                name: "GoverningBodyAdminRole",
                table: "GoverningBodyAdministrations");

            migrationBuilder.AlterColumn<int>(
                name: "GoverningBodyId",
                table: "GoverningBodyAdministrations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations",
                column: "GoverningBodyId",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
