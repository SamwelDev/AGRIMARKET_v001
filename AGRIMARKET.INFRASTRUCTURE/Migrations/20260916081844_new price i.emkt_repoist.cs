using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGRIMARKET.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class newpriceiemkt_repoist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Markets_ModelId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_ModelId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_MarketId",
                table: "Prices",
                column: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Markets_MarketId",
                table: "Prices",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Markets_MarketId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_MarketId",
                table: "Prices");

            migrationBuilder.AddColumn<long>(
                name: "ModelId",
                table: "Prices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ModelId",
                table: "Prices",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Markets_ModelId",
                table: "Prices",
                column: "ModelId",
                principalTable: "Markets",
                principalColumn: "Id");
        }
    }
}
