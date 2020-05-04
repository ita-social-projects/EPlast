using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Make_UserId_CityId_AdminTypeId_NotNull_CityAdministrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_AdminTypes_AdminTypeID",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_Cities_CityID",
                table: "CityAdministrations");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "CityAdministrations",
                newName: "CityId");

            migrationBuilder.RenameColumn(
                name: "AdminTypeID",
                table: "CityAdministrations",
                newName: "AdminTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministrations_CityID",
                table: "CityAdministrations",
                newName: "IX_CityAdministrations_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministrations_AdminTypeID",
                table: "CityAdministrations",
                newName: "IX_CityAdministrations_AdminTypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CityAdministrations",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "CityAdministrations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdminTypeId",
                table: "CityAdministrations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AnnualReports",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_AdminTypes_AdminTypeId",
                table: "CityAdministrations",
                column: "AdminTypeId",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_Cities_CityId",
                table: "CityAdministrations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_AdminTypes_AdminTypeId",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_Cities_CityId",
                table: "CityAdministrations");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "CityAdministrations",
                newName: "CityID");

            migrationBuilder.RenameColumn(
                name: "AdminTypeId",
                table: "CityAdministrations",
                newName: "AdminTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministrations_CityId",
                table: "CityAdministrations",
                newName: "IX_CityAdministrations_CityID");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministrations_AdminTypeId",
                table: "CityAdministrations",
                newName: "IX_CityAdministrations_AdminTypeID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CityAdministrations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityID",
                table: "CityAdministrations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AdminTypeID",
                table: "CityAdministrations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AnnualReports",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_AdminTypes_AdminTypeID",
                table: "CityAdministrations",
                column: "AdminTypeID",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_Cities_CityID",
                table: "CityAdministrations",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
