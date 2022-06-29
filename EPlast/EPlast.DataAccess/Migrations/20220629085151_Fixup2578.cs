using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Fixup2578 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "Referal",
                table: "UserProfiles",
                maxLength: 2560,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "ID", "Name" },
                values: new object[] { 7, "Не маю бажання вказувати" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "Referal",
                table: "UserProfiles");

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "ID", "Name" },
                values: new object[] { 3, "Не маю бажання вказувати" });
        }
    }
}
