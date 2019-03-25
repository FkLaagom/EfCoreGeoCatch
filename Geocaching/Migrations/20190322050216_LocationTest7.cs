using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LocationTest7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Places",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Places",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Places",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Places",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
