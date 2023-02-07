using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DekorEvStartUpFinal.Migrations
{
    public partial class PaymentDateColumnsAddedToProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FrontedPaymentDate",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumPaymentDate",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VipPaymentDate",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontedPaymentDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PremiumPaymentDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VipPaymentDate",
                table: "Products");
        }
    }
}
