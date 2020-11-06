using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Event_Section_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports");

            migrationBuilder.AlterColumn<string>(
                name: "ForWhom",
                table: "Events",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EventCategoryName",
                table: "EventCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "EventSectionId",
                table: "EventCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClubID",
                table: "ClubAnnualReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EventSection",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventSectionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSection", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_EventSectionId",
                table: "EventCategories",
                column: "EventSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCategories_EventSection_EventSectionId",
                table: "EventCategories",
                column: "EventSectionId",
                principalTable: "EventSection",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCategories_EventSection_EventSectionId",
                table: "EventCategories");

            migrationBuilder.DropTable(
                name: "EventSection");

            migrationBuilder.DropIndex(
                name: "IX_EventCategories_EventSectionId",
                table: "EventCategories");

            migrationBuilder.DropColumn(
                name: "EventSectionId",
                table: "EventCategories");

            migrationBuilder.AlterColumn<string>(
                name: "ForWhom",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "EventCategoryName",
                table: "EventCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClubID",
                table: "ClubAnnualReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
