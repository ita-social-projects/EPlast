using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddUpuDegrees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpuDegreeID",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UpuDegrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpuDegrees", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UpuDegreeID",
                table: "UserProfiles",
                column: "UpuDegreeID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                table: "UserProfiles",
                column: "UpuDegreeID",
                principalTable: "UpuDegrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_UpuDegrees_UpuDegreeID",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UpuDegrees");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_UpuDegreeID",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UpuDegreeID",
                table: "UserProfiles");
        }
    }
}
