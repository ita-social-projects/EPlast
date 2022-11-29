using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class MembershipDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateMembership",
                table: "UserMembershipDates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateMembership",
                table: "UserMembershipDates");
        }
    }
}
