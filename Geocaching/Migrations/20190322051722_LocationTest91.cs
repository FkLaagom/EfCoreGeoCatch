using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LocationTest91 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LongitudeHEHE",
                table: "Persons",
                newName: "Locations_Longitude");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Locations_Longitude",
                table: "Persons",
                newName: "LongitudeHEHE");
        }
    }
}
