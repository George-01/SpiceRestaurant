using Microsoft.EntityFrameworkCore.Migrations;

namespace SpiceRestaurant.Data.Migrations
{
    public partial class UpdatedShoppingCart_ApplicationUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ShoppingCart",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "ShoppingCart",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
