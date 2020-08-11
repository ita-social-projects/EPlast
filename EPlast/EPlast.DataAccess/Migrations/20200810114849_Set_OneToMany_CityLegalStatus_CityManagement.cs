using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Set_OneToMany_CityLegalStatus_CityManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements",
                column: "CityLegalStatusOldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements",
                column: "CityLegalStatusOldId",
                unique: true,
                filter: "[CityLegalStatusOldId] IS NOT NULL");
        }
    }
}
