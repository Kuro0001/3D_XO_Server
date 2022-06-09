using Microsoft.EntityFrameworkCore.Migrations;

namespace _3D_XO_Server.Migrations
{
    public partial class Table_Game_Update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "NewsBlocks");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "NewsBlocks",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "imageName",
                table: "NewsBlocks",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "imageContent",
                table: "NewsBlocks",
                newName: "ImageContent");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NewsBlocks",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "NewsBlocks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "NewsBlocks");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "NewsBlocks",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "NewsBlocks",
                newName: "imageName");

            migrationBuilder.RenameColumn(
                name: "ImageContent",
                table: "NewsBlocks",
                newName: "imageContent");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NewsBlocks",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "NewsBlocks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
