using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ClubsReworked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubId",
                table: "ClubAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                table: "ClubAdministrations");

            migrationBuilder.DropIndex(
                name: "IX_ClubAdministrations_ClubMembersID",
                table: "ClubAdministrations");

            migrationBuilder.DropColumn(
                name: "ClubName",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ClubMembersID",
                table: "ClubAdministrations");

            migrationBuilder.AlterColumn<string>(
                name: "ClubURL",
                table: "Clubs",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clubs",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Clubs",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clubs",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "Clubs",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Clubs",
                maxLength: 18,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostIndex",
                table: "Clubs",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Clubs",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ClubMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ClubMembers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ClubMembers",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "ClubAdministrations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ClubAdministrations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClubID",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClubDocumentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDocumentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClubLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateFinish = table.Column<DateTime>(nullable: true),
                    LegalStatusType = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    ClubID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubLegalStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubLegalStatuses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubLegalStatuses_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(maxLength: 120, nullable: false),
                    ClubDocumentTypeId = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubDocuments_ClubDocumentTypes_ClubDocumentTypeId",
                        column: x => x.ClubDocumentTypeId,
                        principalTable: "ClubDocumentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubDocuments_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_UserId",
                table: "ClubAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_ClubID",
                table: "AnnualReports",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDocuments_ClubDocumentTypeId",
                table: "ClubDocuments",
                column: "ClubDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDocuments_ClubId",
                table: "ClubDocuments",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubLegalStatuses_CityId",
                table: "ClubLegalStatuses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubLegalStatuses_ClubID",
                table: "ClubLegalStatuses",
                column: "ClubID");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_Clubs_ClubID",
                table: "AnnualReports",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubId",
                table: "ClubAdministrations",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_AspNetUsers_UserId",
                table: "ClubAdministrations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_Clubs_ClubID",
                table: "AnnualReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubId",
                table: "ClubAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_AspNetUsers_UserId",
                table: "ClubAdministrations");

            migrationBuilder.DropTable(
                name: "ClubDocuments");

            migrationBuilder.DropTable(
                name: "ClubLegalStatuses");

            migrationBuilder.DropTable(
                name: "ClubDocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_ClubAdministrations_UserId",
                table: "ClubAdministrations");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_ClubID",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "PostIndex",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ClubMembers");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ClubMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ClubAdministrations");

            migrationBuilder.DropColumn(
                name: "ClubID",
                table: "AnnualReports");

            migrationBuilder.AlterColumn<string>(
                name: "ClubURL",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubName",
                table: "Clubs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ClubMembers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "ClubAdministrations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ClubMembersID",
                table: "ClubAdministrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubMembersID",
                table: "ClubAdministrations",
                column: "ClubMembersID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_Clubs_ClubId",
                table: "ClubAdministrations",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                table: "ClubAdministrations",
                column: "ClubMembersID",
                principalTable: "ClubMembers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
