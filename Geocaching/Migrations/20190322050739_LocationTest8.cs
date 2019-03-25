using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LocationTest8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.AddColumn<double>(
                name: "Person_Latitude",
                table: "Persons",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Person_Longitude",
                table: "Persons",
                nullable: false,
                defaultValue: 0.0);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Person_Latitude",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Person_Longitude",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Locations_Altitude",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Locations_AltitudeReference",
                table: "Persons");

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Age = table.Column<int>(nullable: false),
                    Location_Altitude = table.Column<double>(nullable: false),
                    Location_AltitudeReference = table.Column<int>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.ID);
                });
        }
    }
}
