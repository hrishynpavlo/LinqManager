using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class CreatePublishingHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublishingHouse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishingHouse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookPublishingHouse",
                columns: table => new
                {
                    BookId = table.Column<int>(nullable: false),
                    PublishingHouseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPublishingHouse", x => new { x.BookId, x.PublishingHouseId });
                    table.ForeignKey(
                        name: "FK_BookPublishingHouse_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookPublishingHouse_PublishingHouse_PublishingHouseId",
                        column: x => x.PublishingHouseId,
                        principalTable: "PublishingHouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookPublishingHouse_PublishingHouseId",
                table: "BookPublishingHouse",
                column: "PublishingHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookPublishingHouse");

            migrationBuilder.DropTable(
                name: "PublishingHouse");
        }
    }
}
