using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Drop_CityLegalStatusTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityLegalStatuses_CityLegalStatusTypes_CityLegalStatusTypeId",
                table: "CityLegalStatuses");

            migrationBuilder.DropTable(
                name: "CityLegalStatusTypes");

            migrationBuilder.DropIndex(
                name: "IX_CityLegalStatuses_CityLegalStatusTypeId",
                table: "CityLegalStatuses");

            migrationBuilder.DropColumn(
                name: "CityLegalStatusTypeId",
                table: "CityLegalStatuses");

            migrationBuilder.AddColumn<int>(
                name: "LegalStatusType",
                table: "CityLegalStatuses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalStatusType",
                table: "CityLegalStatuses");

            migrationBuilder.AddColumn<int>(
                name: "CityLegalStatusTypeId",
                table: "CityLegalStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CityLegalStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLegalStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityLegalStatuses_CityLegalStatusTypeId",
                table: "CityLegalStatuses",
                column: "CityLegalStatusTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CityLegalStatuses_CityLegalStatusTypes_CityLegalStatusTypeId",
                table: "CityLegalStatuses",
                column: "CityLegalStatusTypeId",
                principalTable: "CityLegalStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
