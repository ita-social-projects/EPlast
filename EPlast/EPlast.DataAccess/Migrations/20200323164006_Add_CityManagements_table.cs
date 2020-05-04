using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_CityManagements_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_AspNetUsers_UserId",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_Cities_CityID",
                table: "CityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlastDegrees_AspNetUsers_UserId",
                table: "UserPlastDegrees");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "CityMembers",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_CityMembers_CityID",
                table: "CityMembers",
                newName: "IX_CityMembers_CityId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPlastDegrees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CityMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "CityMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "CityLegalStatuses",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CityAdministrations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CityAdministrations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CityManagements",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityLegalStatus = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    AnnualReportId = table.Column<int>(nullable: false)
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
                        name: "FK_CityManagements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_AnnualReportId",
                table: "CityManagements",
                column: "AnnualReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_UserId",
                table: "CityManagements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_AspNetUsers_UserId",
                table: "CityAdministrations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_Cities_CityId",
                table: "CityMembers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlastDegrees_AspNetUsers_UserId",
                table: "UserPlastDegrees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_AspNetUsers_UserId",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_Cities_CityId",
                table: "CityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlastDegrees_AspNetUsers_UserId",
                table: "UserPlastDegrees");

            migrationBuilder.DropTable(
                name: "CityManagements");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "CityMembers",
                newName: "CityID");

            migrationBuilder.RenameIndex(
                name: "IX_CityMembers_CityId",
                table: "CityMembers",
                newName: "IX_CityMembers_CityID");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPlastDegrees",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CityMembers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CityID",
                table: "CityMembers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateStart",
                table: "CityLegalStatuses",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CityAdministrations",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "CityAdministrations",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_AspNetUsers_UserId",
                table: "CityAdministrations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_Cities_CityID",
                table: "CityMembers",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlastDegrees_AspNetUsers_UserId",
                table: "UserPlastDegrees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
