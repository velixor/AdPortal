using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdPortalApi.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<Guid>(),
                    Name = table.Column<string>()
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Ads",
                table => new
                {
                    Id = table.Column<Guid>(),
                    Number = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(),
                    Content = table.Column<string>(),
                    ImagePath = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false, defaultValue: 0),
                    CreationDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads", x => x.Id);
                    table.ForeignKey(
                        "FK_Ads_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Ads_Number",
                "Ads",
                "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Ads_UserId",
                "Ads",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_Users_Name",
                "Users",
                "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Ads");

            migrationBuilder.DropTable(
                "Users");
        }
    }
}