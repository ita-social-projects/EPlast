using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class DeleteObjectTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("UserTableObjects");
            migrationBuilder.DropTable("AnnualReportTableObjects");
            migrationBuilder.DropTable("ClubAnnualReportTableObjects");
            migrationBuilder.DropTable("RegionAnnualReportTableObjects");
            migrationBuilder.DropTable("UserDistinctionsTableObject");
            migrationBuilder.DropTable("DecisionTableObject");
            migrationBuilder.DropTable("UserPrecautionsTableObject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
