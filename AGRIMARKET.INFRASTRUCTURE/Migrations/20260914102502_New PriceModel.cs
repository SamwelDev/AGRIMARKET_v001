using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGRIMARKET.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class NewPriceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxPrice",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinPrice",
                table: "Prices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Prices");
        }
    }
}
