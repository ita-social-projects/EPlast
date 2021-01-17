using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddUserLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "UserProfiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "UserProfiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {       
            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "UserProfiles");           
        }
    }
}
