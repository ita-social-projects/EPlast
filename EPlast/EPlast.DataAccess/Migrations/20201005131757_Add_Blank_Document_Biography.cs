using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_Blank_Document_Biography : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlankBiographyDocumentsType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlankBiographyDocumentsType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BlankBiographyDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(maxLength: 120, nullable: false),
                    BlankDocumentTypeId = table.Column<int>(nullable: false),
                    BlankBiographyDocumentsTypeID = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlankBiographyDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BlankBiographyDocuments_BlankBiographyDocumentsType_BlankBiographyDocumentsTypeID",
                        column: x => x.BlankBiographyDocumentsTypeID,
                        principalTable: "BlankBiographyDocumentsType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlankBiographyDocuments_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlankBiographyDocuments_BlankBiographyDocumentsTypeID",
                table: "BlankBiographyDocuments",
                column: "BlankBiographyDocumentsTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BlankBiographyDocuments_UserId1",
                table: "BlankBiographyDocuments",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlankBiographyDocuments");

            migrationBuilder.DropTable(
                name: "BlankBiographyDocumentsType");
        }
    }
}
