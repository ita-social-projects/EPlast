using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddColumnEmailSendedForgotPasswordInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailSendedAfterRegistration",
                table: "AspNetUsers",
                newName: "EmailSendedOnRegister");

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailSendedOnForgotPassword",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSendedOnForgotPassword",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EmailSendedOnRegister",
                table: "AspNetUsers",
                newName: "EmailSendedAfterRegistration");
        }
    }
}
