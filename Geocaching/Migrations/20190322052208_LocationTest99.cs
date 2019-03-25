using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LocationTest99 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "LatitudeHEHE",
                table: "Persons",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "LatitudeHEHE",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");
        }
    }
}
