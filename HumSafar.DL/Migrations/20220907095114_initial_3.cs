using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumSafar.DL.Migrations
{
    public partial class initial_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HumSafarUserId = table.Column<string>(nullable: true),
                    TransportName = table.Column<string>(nullable: true),
                    TransportNumber = table.Column<string>(nullable: true),
                    TourCategoryId = table.Column<int>(nullable: false),
                    Origin = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    DateOfJourney = table.Column<DateTime>(nullable: true),
                    Fare = table.Column<double>(nullable: false),
                    OriginImage = table.Column<string>(nullable: true),
                    DestinationImage = table.Column<string>(nullable: true),
                    PassengersName = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<string>(nullable: true),
                    BookingStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_HumSafarUserId",
                        column: x => x.HumSafarUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_TourCategories_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategories",
                        principalColumn: "TravelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HumSafarUserId",
                table: "Bookings",
                column: "HumSafarUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourCategoryId",
                table: "Bookings",
                column: "TourCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
