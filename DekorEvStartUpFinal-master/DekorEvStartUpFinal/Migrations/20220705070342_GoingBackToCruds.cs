using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class GoingBackToCruds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductMainImage",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Products",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductColorMaterials",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductColorMaterials");

            migrationBuilder.AddColumn<string>(
                name: "ProductMainImage",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
