using Microsoft.EntityFrameworkCore.Migrations;

namespace AdPortalApi.Data.Migrations
{
    public partial class removeNumberUniq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ads_Number",
                table: "Ads");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_Number",
                table: "Ads",
                column: "Number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ads_Number",
                table: "Ads");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_Number",
                table: "Ads",
                column: "Number",
                unique: true);
        }
    }
}
