using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class AppUserVerificationTokenCreatedInAppUSerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "AspNetUsers");
        }
    }
}
