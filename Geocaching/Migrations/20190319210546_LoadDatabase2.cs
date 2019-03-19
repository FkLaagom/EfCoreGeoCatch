using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class LoadDatabase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geocashes_Persons_PersonId",
                table: "Geocashes");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Geocashes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Geocashes_Persons_PersonId",
                table: "Geocashes",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geocashes_Persons_PersonId",
                table: "Geocashes");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Geocashes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Geocashes_Persons_PersonId",
                table: "Geocashes",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
