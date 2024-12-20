using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WrocRide.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRideAndRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_RideId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByClientId",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RideId",
                table: "Ratings",
                column: "RideId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_RideId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CreatedByClientId",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RideId",
                table: "Ratings",
                column: "RideId");
        }
    }
}
