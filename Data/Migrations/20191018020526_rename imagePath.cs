using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class renameimagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Ads");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Ads",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Ads");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Ads",
                type: "text",
                nullable: true);
        }
    }
}
