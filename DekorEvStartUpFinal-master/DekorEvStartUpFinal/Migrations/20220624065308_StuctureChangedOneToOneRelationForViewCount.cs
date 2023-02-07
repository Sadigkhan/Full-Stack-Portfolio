using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class StuctureChangedOneToOneRelationForViewCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViewCounts_ProductId",
                table: "ViewCounts");

            migrationBuilder.CreateIndex(
                name: "IX_ViewCounts_ProductId",
                table: "ViewCounts",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViewCounts_ProductId",
                table: "ViewCounts");

            migrationBuilder.CreateIndex(
                name: "IX_ViewCounts_ProductId",
                table: "ViewCounts",
                column: "ProductId");
        }
    }
}
