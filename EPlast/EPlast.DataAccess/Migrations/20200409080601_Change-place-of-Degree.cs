using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangeplaceofDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Degrees_DegreeId",
                table: "Educations");

            migrationBuilder.DropIndex(
                name: "IX_Educations_DegreeId",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Educations");

            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DegreeId",
                table: "UserProfiles",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Degrees_DegreeId",
                table: "UserProfiles",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Degrees_DegreeId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_DegreeId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "UserProfiles");

            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "Educations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_DegreeId",
                table: "Educations",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Degrees_DegreeId",
                table: "Educations",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
