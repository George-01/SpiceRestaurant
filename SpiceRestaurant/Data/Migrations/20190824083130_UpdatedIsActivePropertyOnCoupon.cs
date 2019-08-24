using Microsoft.EntityFrameworkCore.Migrations;

namespace SpiceRestaurant.Data.Migrations
{
    public partial class UpdatedIsActivePropertyOnCoupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Coupon",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Coupon",
                newName: "isActive");
        }
    }
}
