using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangedCityDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeID",
                table: "CityDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_Cities_CityID",
                table: "CityDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentURL",
                table: "CityDocuments");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "CityDocuments",
                newName: "CityId");

            migrationBuilder.RenameColumn(
                name: "CityDocumentTypeID",
                table: "CityDocuments",
                newName: "CityDocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CityDocuments_CityID",
                table: "CityDocuments",
                newName: "IX_CityDocuments_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_CityDocuments_CityDocumentTypeID",
                table: "CityDocuments",
                newName: "IX_CityDocuments_CityDocumentTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "CityDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityDocumentTypeId",
                table: "CityDocuments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CityDocuments",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeId",
                table: "CityDocuments",
                column: "CityDocumentTypeId",
                principalTable: "CityDocumentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_Cities_CityId",
                table: "CityDocuments",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeId",
                table: "CityDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_Cities_CityId",
                table: "CityDocuments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CityDocuments");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "CityDocuments",
                newName: "CityID");

            migrationBuilder.RenameColumn(
                name: "CityDocumentTypeId",
                table: "CityDocuments",
                newName: "CityDocumentTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_CityDocuments_CityId",
                table: "CityDocuments",
                newName: "IX_CityDocuments_CityID");

            migrationBuilder.RenameIndex(
                name: "IX_CityDocuments_CityDocumentTypeId",
                table: "CityDocuments",
                newName: "IX_CityDocuments_CityDocumentTypeID");

            migrationBuilder.AlterColumn<int>(
                name: "CityID",
                table: "CityDocuments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CityDocumentTypeID",
                table: "CityDocuments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "DocumentURL",
                table: "CityDocuments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeID",
                table: "CityDocuments",
                column: "CityDocumentTypeID",
                principalTable: "CityDocumentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_Cities_CityID",
                table: "CityDocuments",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
