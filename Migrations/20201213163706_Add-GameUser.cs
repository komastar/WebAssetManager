using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace AssetWebManager.Migrations
{
    public partial class AddGameUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameModel",
                table: "GameModel");

            migrationBuilder.RenameTable(
                name: "GameModel",
                newName: "GameRoom");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRoom",
                table: "GameRoom",
                column: "GameCode");

            migrationBuilder.CreateTable(
                name: "GameUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    GameRoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUser", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRoom",
                table: "GameRoom");

            migrationBuilder.RenameTable(
                name: "GameRoom",
                newName: "GameModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameModel",
                table: "GameModel",
                column: "GameCode");
        }
    }
}
