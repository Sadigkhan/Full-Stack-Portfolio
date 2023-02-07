using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class MarketImageChangedToUserImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "AspNetUsers",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "MarketImage",
                table: "AspNetUsers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
