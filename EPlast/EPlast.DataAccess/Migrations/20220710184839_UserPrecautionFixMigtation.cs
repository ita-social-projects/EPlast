using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class UserPrecautionFixMigtation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPrecautionsTableObject");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserPrecautions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserPrecautions");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "UserPrecautions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonthsPeriod",
                table: "Precautions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 1,
                column: "MonthsPeriod",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 2,
                column: "MonthsPeriod",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Precautions",
                keyColumn: "Id",
                keyValue: 3,
                column: "MonthsPeriod",
                value: 12);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthsPeriod",
                table: "Precautions");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserPrecautions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserPrecautions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserPrecautions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserPrecautionsTableObject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    PrecautionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reporter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrecautionsTableObject", x => x.Id);
                });
        }
    }
}
