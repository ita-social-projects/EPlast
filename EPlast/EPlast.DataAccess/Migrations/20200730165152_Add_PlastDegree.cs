using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_PlastDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPlastDegreeType",
                table: "UserPlastDegrees");

            migrationBuilder.AddColumn<int>(
                name: "PlastDegreeId",
                table: "UserPlastDegrees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlastDegrees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlastDegrees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_PlastDegreeId",
                table: "UserPlastDegrees",
                column: "PlastDegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlastDegrees_PlastDegrees_PlastDegreeId",
                table: "UserPlastDegrees",
                column: "PlastDegreeId",
                principalTable: "PlastDegrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlastDegrees_PlastDegrees_PlastDegreeId",
                table: "UserPlastDegrees");

            migrationBuilder.DropTable(
                name: "PlastDegrees");

            migrationBuilder.DropIndex(
                name: "IX_UserPlastDegrees_PlastDegreeId",
                table: "UserPlastDegrees");

            migrationBuilder.DropColumn(
                name: "PlastDegreeId",
                table: "UserPlastDegrees");

            migrationBuilder.AddColumn<int>(
                name: "UserPlastDegreeType",
                table: "UserPlastDegrees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
