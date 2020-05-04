using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Drop_UserPlastDegreeTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlastDegrees_UserPlastDegreeTypes_UserPlastDegreeTypeId",
                table: "UserPlastDegrees");

            migrationBuilder.DropTable(
                name: "UserPlastDegreeTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserPlastDegrees_UserPlastDegreeTypeId",
                table: "UserPlastDegrees");

            migrationBuilder.DropColumn(
                name: "UserPlastDegreeTypeId",
                table: "UserPlastDegrees");

            migrationBuilder.AddColumn<int>(
                name: "UserPlastDegreeType",
                table: "UserPlastDegrees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPlastDegreeType",
                table: "UserPlastDegrees");

            migrationBuilder.AddColumn<int>(
                name: "UserPlastDegreeTypeId",
                table: "UserPlastDegrees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserPlastDegreeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlastDegreeTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_UserPlastDegreeTypeId",
                table: "UserPlastDegrees",
                column: "UserPlastDegreeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlastDegrees_UserPlastDegreeTypes_UserPlastDegreeTypeId",
                table: "UserPlastDegrees",
                column: "UserPlastDegreeTypeId",
                principalTable: "UserPlastDegreeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
