using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations
{
    public partial class CriateTableGameResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameResultId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameResultId",
                table: "Games",
                column: "GameResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameResults_GameResultId",
                table: "Games",
                column: "GameResultId",
                principalTable: "GameResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameResults_GameResultId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "GameResults");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameResultId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameResultId",
                table: "Games");
        }
    }
}
