using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddEventTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministrationType",
                table: "EventAdministration");

            migrationBuilder.AddColumn<int>(
                name: "EventAdministrationTypeID",
                table: "EventAdministration",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventAdministrationType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventAdministrationTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAdministrationType", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAdministration_EventAdministrationTypeID",
                table: "EventAdministration",
                column: "EventAdministrationTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_EventAdministrationType_EventAdministrationTypeID",
                table: "EventAdministration",
                column: "EventAdministrationTypeID",
                principalTable: "EventAdministrationType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_EventAdministrationType_EventAdministrationTypeID",
                table: "EventAdministration");

            migrationBuilder.DropTable(
                name: "EventAdministrationType");

            migrationBuilder.DropIndex(
                name: "IX_EventAdministration_EventAdministrationTypeID",
                table: "EventAdministration");

            migrationBuilder.DropColumn(
                name: "EventAdministrationTypeID",
                table: "EventAdministration");

            migrationBuilder.AddColumn<string>(
                name: "AdministrationType",
                table: "EventAdministration",
                nullable: true);
        }
    }
}
