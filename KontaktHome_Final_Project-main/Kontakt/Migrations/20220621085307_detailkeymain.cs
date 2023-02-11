using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontakt.Migrations
{
    public partial class detailkeymain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isMain",
                table: "DetailKeys",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isMain",
                table: "DetailKeys");
        }
    }
}
