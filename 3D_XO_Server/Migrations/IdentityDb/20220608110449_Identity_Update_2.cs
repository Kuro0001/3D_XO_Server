using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations.IdentityDb
{
    public partial class Identity_Update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Violation",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Violation",
                table: "AspNetUsers");
        }
    }
}
