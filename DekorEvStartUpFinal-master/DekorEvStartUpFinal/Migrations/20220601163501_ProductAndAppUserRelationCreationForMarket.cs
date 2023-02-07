using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class ProductAndAppUserRelationCreationForMarket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AppUserId",
                table: "Products",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_AppUserId",
                table: "Products",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_AppUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AppUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Products");
        }
    }
}
