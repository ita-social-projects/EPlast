using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Set_OneToMany_CityAdministration_CityManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements",
                column: "CityAdminOldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements",
                column: "CityAdminOldId",
                unique: true,
                filter: "[CityAdminOldId] IS NOT NULL");
        }
    }
}
