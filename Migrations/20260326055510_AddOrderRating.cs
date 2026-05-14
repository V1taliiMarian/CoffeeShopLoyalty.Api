using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopLoyalty.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Orders");
        }
    }
}
