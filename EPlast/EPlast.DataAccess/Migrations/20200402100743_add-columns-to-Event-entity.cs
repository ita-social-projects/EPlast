using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class addcolumnstoEvententity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForWhom",
                table: "Events",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormOfHolding",
                table: "Events",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPartisipants",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Questions",
                table: "Events",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForWhom",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FormOfHolding",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NumberOfPartisipants",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Questions",
                table: "Events");
        }
    }
}
