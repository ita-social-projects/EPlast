using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class achimentdocuments_many_to_many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AchievementDocuments_AspNetUsers_UserId",
                table: "AchievementDocuments");

            migrationBuilder.DropTable(
                name: "UserCourses");

            migrationBuilder.DropTable(
                name: "UserTableObjects");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "AchievementDocuments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementDocuments_CourseId",
                table: "AchievementDocuments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementDocuments_Courses_CourseId",
                table: "AchievementDocuments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementDocuments_AspNetUsers_UserId",
                table: "AchievementDocuments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AchievementDocuments_Courses_CourseId",
                table: "AchievementDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_AchievementDocuments_AspNetUsers_UserId",
                table: "AchievementDocuments");

            migrationBuilder.DropIndex(
                name: "IX_AchievementDocuments_CourseId",
                table: "AchievementDocuments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "AchievementDocuments");

            migrationBuilder.CreateTable(
                name: "UserCourses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StatusPassedCourse = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTableObjects",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClubId = table.Column<int>(type: "int", nullable: true),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DegreeId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Oblast = table.Column<byte>(type: "tinyint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlastDegree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UPUDegree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_CourseId",
                table: "UserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_UserId",
                table: "UserCourses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementDocuments_AspNetUsers_UserId",
                table: "AchievementDocuments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
