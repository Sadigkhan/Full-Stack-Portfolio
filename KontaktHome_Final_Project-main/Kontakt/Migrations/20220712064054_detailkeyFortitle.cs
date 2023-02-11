using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontakt.Migrations
{
    public partial class detailkeyFortitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForTitle",
                table: "DetailKeys",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForTitle",
                table: "DetailKeys");
        }
    }
}
