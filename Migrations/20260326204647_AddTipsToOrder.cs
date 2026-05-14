using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopLoyalty.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTipsToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TipAmount",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipAmount",
                table: "Orders");
        }
    }
}
