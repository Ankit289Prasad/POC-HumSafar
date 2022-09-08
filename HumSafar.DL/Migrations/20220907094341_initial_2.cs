using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumSafar.DL.Migrations
{
    public partial class initial_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    TourId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportName = table.Column<string>(nullable: true),
                    TransportNumber = table.Column<string>(nullable: true),
                    TourCategoryId = table.Column<int>(nullable: false),
                    Origin = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    DateOfJourney = table.Column<DateTime>(nullable: true),
                    Fare = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    SeatAvailable = table.Column<int>(nullable: false),
                    AvailabilityStatus = table.Column<string>(nullable: true),
                    OriginImage = table.Column<string>(nullable: true),
                    DestinationImage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_Tours_TourCategories_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategories",
                        principalColumn: "TravelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_TourCategoryId",
                table: "Tours",
                column: "TourCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tours");
        }
    }
}
