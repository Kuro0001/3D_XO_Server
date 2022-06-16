using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations
{
    public partial class CorrectTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameResults_GameResultId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameResultId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameResultId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ResultId",
                table: "Games",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameResults_ResultId",
                table: "Games",
                column: "ResultId",
                principalTable: "GameResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameResults_ResultId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ResultId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "GameResultId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
