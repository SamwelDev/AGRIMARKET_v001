using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGRIMARKET.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Regions_RegionModelId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_RegionModelId",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "RegionModelId",
                table: "Districts");

            migrationBuilder.AddColumn<long>(
                name: "RegionId",
                table: "Districts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_RegionId",
                table: "Districts",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Regions_RegionId",
                table: "Districts",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Regions_RegionId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_RegionId",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Districts");

            migrationBuilder.AddColumn<long>(
                name: "RegionModelId",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_RegionModelId",
                table: "Districts",
                column: "RegionModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Regions_RegionModelId",
                table: "Districts",
                column: "RegionModelId",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}
