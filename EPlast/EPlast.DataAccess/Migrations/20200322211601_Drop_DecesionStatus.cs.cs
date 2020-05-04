using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Drop_DecesionStatuscs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_DecesionStatuses_DecesionStatusID",
                table: "Decesions");

            migrationBuilder.DropTable(
                name: "DecesionStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Decesions_DecesionStatusID",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "DecesionStatusID",
                table: "Decesions");

            migrationBuilder.AddColumn<int>(
                name: "DecesionStatus",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecesionStatus",
                table: "Decesions");

            migrationBuilder.AddColumn<int>(
                name: "DecesionStatusID",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DecesionStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DecesionStatusName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecesionStatuses", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_DecesionStatusID",
                table: "Decesions",
                column: "DecesionStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_DecesionStatuses_DecesionStatusID",
                table: "Decesions",
                column: "DecesionStatusID",
                principalTable: "DecesionStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
