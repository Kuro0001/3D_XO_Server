using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations
{
    public partial class Update_all_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageContent",
                table: "NewsBlocks");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "NewsBlocks");

            migrationBuilder.DropColumn(
                name: "Gamer1Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Gamer2ID",
                table: "Games");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "NewsBlocks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "User1Id",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User2ID",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GamesStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsReaded = table.Column<bool>(nullable: false),
                    GameState = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamesStates_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamesStates_GameId",
                table: "GamesStates",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesStates");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "NewsBlocks");

            migrationBuilder.DropColumn(
                name: "User1Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User2ID",
                table: "Games");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageContent",
                table: "NewsBlocks",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "NewsBlocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gamer1Id",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gamer2ID",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
