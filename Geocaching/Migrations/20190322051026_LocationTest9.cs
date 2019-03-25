using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LocationTest9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Person_Longitude",
                table: "Persons",
                newName: "LongitudeHEHE");

            migrationBuilder.RenameColumn(
                name: "Person_Latitude",
                table: "Persons",
                newName: "LatitudeHEHE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LongitudeHEHE",
                table: "Persons",
                newName: "Person_Longitude");

            migrationBuilder.RenameColumn(
                name: "LatitudeHEHE",
                table: "Persons",
                newName: "Person_Latitude");
        }
    }
}
