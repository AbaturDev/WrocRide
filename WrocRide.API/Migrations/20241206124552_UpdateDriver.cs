using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WrocRide.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_DocumentId",
                table: "Drivers",
                column: "DocumentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Documents_DocumentId",
                table: "Drivers",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Documents_DocumentId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_DocumentId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Drivers");
        }
    }
}
