using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetWebManager.Migrations
{
    public partial class ContentLockAddLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Lock",
                table: "ContentLock",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lock",
                table: "ContentLock");
        }
    }
}
