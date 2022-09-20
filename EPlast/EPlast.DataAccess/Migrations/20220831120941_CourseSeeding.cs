using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class CourseSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "ID", "Link", "Name" },
                values: new object[] { 1, "https://vumonline.ua/search/?search=%D0%BF%D0%BB%D0%B0%D1%81%D1%82", "Vumonline курс" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
