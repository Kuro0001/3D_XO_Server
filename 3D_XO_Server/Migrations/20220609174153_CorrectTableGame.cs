using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations
{
    public partial class CorrectTableGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User2ID",
                table: "Games",
                newName: "User2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "Games",
                newName: "User2ID");
        }
    }
}
