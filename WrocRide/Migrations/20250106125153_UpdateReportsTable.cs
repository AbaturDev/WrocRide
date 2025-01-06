using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WrocRide.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Admins_AdminId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ReportedUserId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReporterUserId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedUserId",
                table: "Reports",
                column: "ReportedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId",
                table: "Reports",
                column: "ReporterUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Admins_AdminId",
                table: "Reports",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReportedUserId",
                table: "Reports",
                column: "ReportedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReporterUserId",
                table: "Reports",
                column: "ReporterUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Admins_AdminId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReportedUserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReporterUserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportedUserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportedUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReporterUserId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Admins_AdminId",
                table: "Reports",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
