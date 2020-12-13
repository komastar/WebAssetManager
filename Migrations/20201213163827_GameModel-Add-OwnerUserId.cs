using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetWebManager.Migrations
{
    public partial class GameModelAddOwnerUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUserId",
                table: "GameRoom",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "GameRoom");
        }
    }
}
