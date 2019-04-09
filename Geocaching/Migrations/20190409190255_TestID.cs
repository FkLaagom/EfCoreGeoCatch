using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class TestID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geocashes_Persons_PersonID",
                table: "Geocashes");

            migrationBuilder.RenameColumn(
                name: "PersonID",
                table: "Geocashes",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Geocashes_PersonID",
                table: "Geocashes",
                newName: "IX_Geocashes_PersonId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geocashes_Persons_PersonId",
                table: "Geocashes");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Geocashes",
                newName: "PersonID");

            migrationBuilder.RenameIndex(
                name: "IX_Geocashes_PersonId",
                table: "Geocashes",
                newName: "IX_Geocashes_PersonID");

            migrationBuilder.AlterColumn<int>(
                name: "PersonID",
                table: "Geocashes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Geocashes_Persons_PersonID",
                table: "Geocashes",
                column: "PersonID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
