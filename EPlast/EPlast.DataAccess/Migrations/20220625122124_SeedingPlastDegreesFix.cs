using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class SeedingPlastDegreesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Старший пластун прихильник / Старша пластунка прихильниця");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Старший пластун / Старша пластунка");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Старший пластун скоб / Cтарша пластунка вірлиця");

            migrationBuilder.InsertData(
                table: "PlastDegrees",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Старший пластун гетьманський скоб / Старша пластунка гетьманська вірлиця" },
                    { 5, "Старший пластун скоб гребець / Старша пластунка  вірлиця гребець" },
                    { 6, "Старший пластун скоб обсерватор / Старша пластунка  вірлиця обсерватор" },
                    { 7, "Пластун сеніор прихильник / Пластунка сеніорка прихильниця" },
                    { 8, "Пластун сеніор керівництва / Пластунка сеніорка керівництва" },
                    { 9, "Пластприят" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Пластун сеніор прихильник / Пластунка сеніорка прихильниця");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Пластун сеніор керівництва / Пластунка сеніорка керівництва");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Пластприят");
        }
    }
}
