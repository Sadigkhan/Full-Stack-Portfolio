using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class StructureChangedForAppUserViewCOunt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViewCounts_AppUserId",
                table: "ViewCounts");

            migrationBuilder.CreateIndex(
                name: "IX_ViewCounts_AppUserId",
                table: "ViewCounts",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ViewCounts_AppUserId",
                table: "ViewCounts");

            migrationBuilder.CreateIndex(
                name: "IX_ViewCounts_AppUserId",
                table: "ViewCounts",
                column: "AppUserId");
        }
    }
}
