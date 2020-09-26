using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddedNumberDistinction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "UserDistinctions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "UserDistinctions");
        }
    }
}
