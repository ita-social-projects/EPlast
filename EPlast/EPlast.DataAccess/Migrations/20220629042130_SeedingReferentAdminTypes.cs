using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class SeedingReferentAdminTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminTypes",
                columns: new[] { "ID", "AdminTypeName" },
                values: new object[,]
                {
                    { 24, "Референт/-ка УПС Округи" },
                    { 25, "Референт/-ка УСП Округи" },
                    { 26, "Референт дійсного членства Округи" },
                    { 27, "Референт/-ка УПС Станиці" },
                    { 28, "Референт/-ка УСП Станиці" },
                    { 29, "Референт дійсного членства Станиці" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminTypes",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AdminTypes",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AdminTypes",
                keyColumn: "ID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AdminTypes",
                keyColumn: "ID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AdminTypes",
                keyColumn: "ID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AdminTypes",
                keyColumn: "ID",
                keyValue: 29);
        }
    }
}
