using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RenamedPrecautions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "І пересторога");

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "ІІ пересторога");

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "ІІІ пересторога");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Догана");

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Сувора догана");

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Догана із загрозою виключення з Пласту");
        }
    }
}
