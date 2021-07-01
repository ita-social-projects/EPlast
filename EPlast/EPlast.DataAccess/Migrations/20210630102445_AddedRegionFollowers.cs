using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddedRegionFollowers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegionFollowers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    Appeal = table.Column<string>(maxLength: 1024, nullable: false),
                    CityName = table.Column<string>(maxLength: 50, nullable: false),
                    CityDescription = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: false),
                    Street = table.Column<string>(maxLength: 50, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    OfficeNumber = table.Column<string>(maxLength: 10, nullable: false),
                    PostIndex = table.Column<string>(maxLength: 10, nullable: false),
                    СityURL = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionFollowers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionFollowers_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionFollowers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegionFollowers_RegionId",
                table: "RegionFollowers",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionFollowers_UserId",
                table: "RegionFollowers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionFollowers");
        }
    }
}
