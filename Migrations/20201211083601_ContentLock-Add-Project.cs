using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetWebManager.Migrations
{
    public partial class ContentLockAddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Project",
                table: "ContentLock",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Project",
                table: "ContentLock");
        }
    }
}
