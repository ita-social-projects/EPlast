using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_CityAdminOld_CityLegalStatusOld_to_CityManagements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_Events_EventID",
                table: "EventAdministration");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserID",
                table: "EventAdministration");

            migrationBuilder.DropColumn(
                name: "CityLegalStatus",
                table: "CityManagements");

            migrationBuilder.AlterColumn<string>(
                name: "Questions",
                table: "Events",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "Events",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "EventAdministration",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "EventAdministration",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityAdminOldId",
                table: "CityManagements",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityLegalStatusNew",
                table: "CityManagements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CityLegalStatusOldId",
                table: "CityManagements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements",
                column: "CityAdminOldId",
                unique: true,
                filter: "[CityAdminOldId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements",
                column: "CityLegalStatusOldId",
                unique: true,
                filter: "[CityLegalStatusOldId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CityManagements_CityAdministrations_CityAdminOldId",
                table: "CityManagements",
                column: "CityAdminOldId",
                principalTable: "CityAdministrations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityManagements_CityLegalStatuses_CityLegalStatusOldId",
                table: "CityManagements",
                column: "CityLegalStatusOldId",
                principalTable: "CityLegalStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_Events_EventID",
                table: "EventAdministration",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserID",
                table: "EventAdministration",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityManagements_CityAdministrations_CityAdminOldId",
                table: "CityManagements");

            migrationBuilder.DropForeignKey(
                name: "FK_CityManagements_CityLegalStatuses_CityLegalStatusOldId",
                table: "CityManagements");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_Events_EventID",
                table: "EventAdministration");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserID",
                table: "EventAdministration");

            migrationBuilder.DropIndex(
                name: "IX_CityManagements_CityAdminOldId",
                table: "CityManagements");

            migrationBuilder.DropIndex(
                name: "IX_CityManagements_CityLegalStatusOldId",
                table: "CityManagements");

            migrationBuilder.DropColumn(
                name: "CityAdminOldId",
                table: "CityManagements");

            migrationBuilder.DropColumn(
                name: "CityLegalStatusNew",
                table: "CityManagements");

            migrationBuilder.DropColumn(
                name: "CityLegalStatusOldId",
                table: "CityManagements");

            migrationBuilder.AlterColumn<string>(
                name: "Questions",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "EventAdministration",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "EventAdministration",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CityLegalStatus",
                table: "CityManagements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_Events_EventID",
                table: "EventAdministration",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserID",
                table: "EventAdministration",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
