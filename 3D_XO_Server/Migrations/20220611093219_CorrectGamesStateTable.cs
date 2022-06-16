using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations
{
    public partial class CorrectGamesStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameState",
                table: "GamesStates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GamesStates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "GamesStates",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "GamesStates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "GamesStates");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "GamesStates");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GamesStates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameState",
                table: "GamesStates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
