using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetWebManager.Migrations
{
    public partial class GameUserModelModId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameUser",
                table: "GameUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameUser",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameUser",
                table: "GameUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameUser",
                table: "GameUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameUser",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameUser",
                table: "GameUser",
                column: "Id");
        }
    }
}
