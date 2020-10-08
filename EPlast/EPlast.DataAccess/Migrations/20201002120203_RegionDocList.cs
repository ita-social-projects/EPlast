using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RegionDocList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_RegionDocs_RegionId",
                table: "RegionDocs",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegionDocs_Regions_RegionId",
                table: "RegionDocs",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegionDocs_Regions_RegionId",
                table: "RegionDocs");

            migrationBuilder.DropIndex(
                name: "IX_RegionDocs_RegionId",
                table: "RegionDocs");

           
        }
    }
}
