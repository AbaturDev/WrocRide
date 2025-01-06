using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WrocRide.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRideAndSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BudgetPerRide",
                table: "Schedules",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Distance",
                table: "Schedules",
                type: "decimal(8,3)",
                precision: 8,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Coast",
                table: "Rides",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Distance",
                table: "Rides",
                type: "decimal(8,3)",
                precision: 8,
                scale: 3,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetPerRide",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Rides");

            migrationBuilder.AlterColumn<decimal>(
                name: "Coast",
                table: "Rides",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
