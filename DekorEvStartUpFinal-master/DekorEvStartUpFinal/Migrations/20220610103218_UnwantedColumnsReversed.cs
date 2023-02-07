using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class UnwantedColumnsReversed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Baskets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Baskets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ColorId",
                table: "Baskets",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_MaterialId",
                table: "Baskets",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Colors_ColorId",
                table: "Baskets",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Materials_MaterialId",
                table: "Baskets",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Colors_ColorId",
                table: "Baskets");

            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Materials_MaterialId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_ColorId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_MaterialId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Baskets");
        }
    }
}
