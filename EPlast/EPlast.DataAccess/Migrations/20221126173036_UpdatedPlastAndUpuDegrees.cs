using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class UpdatedPlastAndUpuDegrees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Старший пластун / Старша пластунка");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Старший пластун скоб / Старша пластунка вірлиця");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Старший пластун скоб-гребець / Старша пластунка вірлиця-гребець");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Старший пластун скоб-обсерватор / Старша пластунка вірлиця-обсерватор");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Старший пластун гетьманський скоб / Старша пластунка гетьманський вірлиця");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Старший пластун гетьманський скоб-обсерватор / Старша пластунка гетьманський вірлиця-обсерватор");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Старший пластун гетьманський скоб-гребець / Старша пластункагетьманський вірлиця-гребець");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Сеніор праці / Пластунка сеніорка праці");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Сеніор довіри / Пластунка сеніорка довіри");

            migrationBuilder.InsertData(
                table: "PlastDegrees",
                columns: new[] { "Id", "Name" },
                values: new object[] { 10, "Сеніор керівництва / Пластунка сеніорка керівництва" });

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "не був/-ла у юнацтві");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "пластун/-ка прихильник/-ця");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 3,
                column: "Name",
                value: "пластун/-ка учасник/-ця");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "пластун/-ка розвідувач/-ка");

            migrationBuilder.InsertData(
                table: "UpuDegrees",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 10, "пластун гетьманський скоб-гребець / пластун гетьманська скоб-гребець" },
                    { 9, "пластун гетьманський скоб-обсерватор / пластун гетьманська вірлиця-обсерватор" },
                    { 8, "пластун гетьманський скоб / пластун гетьманська вірлиця" },
                    { 7, "пластун скоб-обсерватор / пластунка вірлиця-обсерватор" },
                    { 6, "пластун скоб-гребець / пластунка вірлиця-гребець" },
                    { 5, "пластун скоб / пластунка вірлиця" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 10);

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

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Старший пластун гетьманський скоб / Старша пластунка гетьманська вірлиця");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Старший пластун скоб гребець / Старша пластунка  вірлиця гребець");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Старший пластун скоб обсерватор / Старша пластунка  вірлиця обсерватор");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Пластун сеніор прихильник / Пластунка сеніорка прихильниця");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Пластун сеніор керівництва / Пластунка сеніорка керівництва");

            migrationBuilder.UpdateData(
                table: "PlastDegrees",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Пластприят");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 1,
                column: "Name",
                value: "не був/-ла в юнацтві");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 2,
                column: "Name",
                value: "пластун/-ка учасник/-ця");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 3,
                column: "Name",
                value: "пластун/-ка розвідувач/-ка");

            migrationBuilder.UpdateData(
                table: "UpuDegrees",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "пластун скоб / пластунка вірлиця");
        }
    }
}
