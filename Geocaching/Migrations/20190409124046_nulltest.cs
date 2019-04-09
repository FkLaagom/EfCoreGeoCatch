using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class nulltest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locations_Altitude",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Locations_AltitudeReference",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "LatitudeHEHE",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Locations_Longitude",
                table: "Persons");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Persons",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Persons",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Geocashes",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Geocashes",
                type: "FLOAT",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Persons",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");

            migrationBuilder.AddColumn<double>(
                name: "Locations_Altitude",
                table: "Persons",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Locations_AltitudeReference",
                table: "Persons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "LatitudeHEHE",
                table: "Persons",
                type: "FLOAT",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Locations_Longitude",
                table: "Persons",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Geocashes",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Geocashes",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "FLOAT");
        }
    }
}
