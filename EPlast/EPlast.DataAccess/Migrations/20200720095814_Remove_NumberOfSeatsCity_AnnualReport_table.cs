using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Remove_NumberOfSeatsCity_AnnualReport_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfSeatsInCity",
                table: "AnnualReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeatsInCity",
                table: "AnnualReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
