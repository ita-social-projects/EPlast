using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RemovedColumnsFromEducatorsStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasisOfGranting",
                table: "KVs");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "KVs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BasisOfGranting",
                table: "KVs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "KVs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
