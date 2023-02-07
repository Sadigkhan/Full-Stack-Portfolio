using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class ProductMaterialColorTableMaterialIdCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "ProductColorMaterials");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "ProductColorMaterials",
                type: "int",
                nullable: true);
        }
    }
}
