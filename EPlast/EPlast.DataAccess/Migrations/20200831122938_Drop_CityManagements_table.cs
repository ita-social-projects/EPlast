using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Drop_CityManagements_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports");

            migrationBuilder.DropTable(
                name: "CityManagements");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_UserId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnnualReports");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewCityAdminId",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewCityLegalStatusType",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_CreatorId",
                table: "AnnualReports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_NewCityAdminId",
                table: "AnnualReports",
                column: "NewCityAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AspNetUsers_CreatorId",
                table: "AnnualReports",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AspNetUsers_NewCityAdminId",
                table: "AnnualReports",
                column: "NewCityAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AspNetUsers_CreatorId",
                table: "AnnualReports");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AspNetUsers_NewCityAdminId",
                table: "AnnualReports");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_CreatorId",
                table: "AnnualReports");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_NewCityAdminId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NewCityAdminId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NewCityLegalStatusType",
                table: "AnnualReports");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AnnualReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CityManagements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnualReportId = table.Column<int>(type: "int", nullable: false),
                    CityAdminOldId = table.Column<int>(type: "int", nullable: true),
                    CityLegalStatusNew = table.Column<int>(type: "int", nullable: false),
                    CityLegalStatusOldId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityManagements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityManagements_AnnualReports_AnnualReportId",
                        column: x => x.AnnualReportId,
                        principalTable: "AnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityManagements_CityAdministrations_CityAdminOldId",
                        column: x => x.CityAdminOldId,
                        principalTable: "CityAdministrations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityManagements_CityLegalStatuses_CityLegalStatusOldId",
                        column: x => x.CityLegalStatusOldId,
                        principalTable: "CityLegalStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityManagements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_UserId",
                table: "AnnualReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_AnnualReportId",
                table: "CityManagements",
                column: "AnnualReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements",
                column: "CityAdminOldId");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements",
                column: "CityLegalStatusOldId");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_UserId",
                table: "CityManagements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
