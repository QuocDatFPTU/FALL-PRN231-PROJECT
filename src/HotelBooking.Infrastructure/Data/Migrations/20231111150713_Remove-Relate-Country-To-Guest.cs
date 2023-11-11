using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelateCountryToGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Countries_CountryId",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Guests_CountryId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Guests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Guests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guests_CountryId",
                table: "Guests",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Countries_CountryId",
                table: "Guests",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }
    }
}
