using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddConstraintGenderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateCheckConstraint(
                name: "constraint_gender",
                table: "Genders",
                sql: "(Name = 'Чоловік' AND ID = 1) OR (Name = 'Жінка' AND ID = 2) OR (Name = 'Не маю бажання вказувати' AND ID = 7)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "constraint_gender",
                table: "Genders");
        }
    }
}
