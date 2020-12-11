using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace AssetWebManager.Migrations
{
    public partial class AddContentLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameModel",
                table: "GameModel");

            migrationBuilder.AlterColumn<string>(
                name: "GameCode",
                table: "GameModel",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameModel",
                table: "GameModel",
                column: "GameCode");

            migrationBuilder.CreateTable(
                name: "ContentLock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLock", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentLock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameModel",
                table: "GameModel");

            migrationBuilder.AlterColumn<string>(
                name: "GameCode",
                table: "GameModel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameModel",
                table: "GameModel",
                column: "Id");
        }
    }
}
