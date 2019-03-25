using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LocationTest5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Places",
                type: "Float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "NUMERIC(38, 16)");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Places",
                type: "Float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "NUMERIC(38, 16)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Places",
                type: "NUMERIC(38, 16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "Float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Places",
                type: "NUMERIC(38, 16)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "Float");
        }
    }
}
