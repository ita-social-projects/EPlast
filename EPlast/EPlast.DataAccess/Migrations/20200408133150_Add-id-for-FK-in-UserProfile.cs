using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddidforFKinUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Degrees_DegreeID",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Educations_EducationID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Nationalities_NationalityID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Religions_ReligionID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Works_WorkID",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "WorkID",
                table: "UserProfiles",
                newName: "WorkId");

            migrationBuilder.RenameColumn(
                name: "ReligionID",
                table: "UserProfiles",
                newName: "ReligionId");

            migrationBuilder.RenameColumn(
                name: "NationalityID",
                table: "UserProfiles",
                newName: "NationalityId");

            migrationBuilder.RenameColumn(
                name: "EducationID",
                table: "UserProfiles",
                newName: "EducationId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_WorkID",
                table: "UserProfiles",
                newName: "IX_UserProfiles_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_ReligionID",
                table: "UserProfiles",
                newName: "IX_UserProfiles_ReligionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_NationalityID",
                table: "UserProfiles",
                newName: "IX_UserProfiles_NationalityId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_EducationID",
                table: "UserProfiles",
                newName: "IX_UserProfiles_EducationId");

            migrationBuilder.RenameColumn(
                name: "DegreeID",
                table: "Educations",
                newName: "DegreeId");

            migrationBuilder.RenameIndex(
                name: "IX_Educations_DegreeID",
                table: "Educations",
                newName: "IX_Educations_DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Degrees_DegreeId",
                table: "Educations",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Educations_EducationId",
                table: "UserProfiles",
                column: "EducationId",
                principalTable: "Educations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Nationalities_NationalityId",
                table: "UserProfiles",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Religions_ReligionId",
                table: "UserProfiles",
                column: "ReligionId",
                principalTable: "Religions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Works_WorkId",
                table: "UserProfiles",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Degrees_DegreeId",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Educations_EducationId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Nationalities_NationalityId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Religions_ReligionId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Works_WorkId",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "WorkId",
                table: "UserProfiles",
                newName: "WorkID");

            migrationBuilder.RenameColumn(
                name: "ReligionId",
                table: "UserProfiles",
                newName: "ReligionID");

            migrationBuilder.RenameColumn(
                name: "NationalityId",
                table: "UserProfiles",
                newName: "NationalityID");

            migrationBuilder.RenameColumn(
                name: "EducationId",
                table: "UserProfiles",
                newName: "EducationID");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_WorkId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_WorkID");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_ReligionId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_ReligionID");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_NationalityId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_NationalityID");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_EducationId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_EducationID");

            migrationBuilder.RenameColumn(
                name: "DegreeId",
                table: "Educations",
                newName: "DegreeID");

            migrationBuilder.RenameIndex(
                name: "IX_Educations_DegreeId",
                table: "Educations",
                newName: "IX_Educations_DegreeID");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "UserProfiles",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Degrees_DegreeID",
                table: "Educations",
                column: "DegreeID",
                principalTable: "Degrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Educations_EducationID",
                table: "UserProfiles",
                column: "EducationID",
                principalTable: "Educations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Nationalities_NationalityID",
                table: "UserProfiles",
                column: "NationalityID",
                principalTable: "Nationalities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Religions_ReligionID",
                table: "UserProfiles",
                column: "ReligionID",
                principalTable: "Religions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Works_WorkID",
                table: "UserProfiles",
                column: "WorkID",
                principalTable: "Works",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
