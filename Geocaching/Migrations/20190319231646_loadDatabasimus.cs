using Microsoft.EntityFrameworkCore.Migrations;

namespace Geocaching.Migrations
{
    public partial class loadDatabasimus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoundGeocaches_Geocashes_GeocasheId",
                table: "FoundGeocaches");

            migrationBuilder.DropForeignKey(
                name: "FK_FoundGeocaches_Persons_PersonId",
                table: "FoundGeocaches");

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

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "FoundGeocaches",
                newName: "PersonID");

            migrationBuilder.RenameColumn(
                name: "GeocasheId",
                table: "FoundGeocaches",
                newName: "GeocasheID");

            migrationBuilder.RenameIndex(
                name: "IX_FoundGeocaches_PersonId",
                table: "FoundGeocaches",
                newName: "IX_FoundGeocaches_PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_FoundGeocaches_Geocashes_GeocasheID",
                table: "FoundGeocaches",
                column: "GeocasheID",
                principalTable: "Geocashes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoundGeocaches_Persons_PersonID",
                table: "FoundGeocaches",
                column: "PersonID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Geocashes_Persons_PersonID",
                table: "Geocashes",
                column: "PersonID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoundGeocaches_Geocashes_GeocasheID",
                table: "FoundGeocaches");

            migrationBuilder.DropForeignKey(
                name: "FK_FoundGeocaches_Persons_PersonID",
                table: "FoundGeocaches");

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

            migrationBuilder.RenameColumn(
                name: "PersonID",
                table: "FoundGeocaches",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "GeocasheID",
                table: "FoundGeocaches",
                newName: "GeocasheId");

            migrationBuilder.RenameIndex(
                name: "IX_FoundGeocaches_PersonID",
                table: "FoundGeocaches",
                newName: "IX_FoundGeocaches_PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoundGeocaches_Geocashes_GeocasheId",
                table: "FoundGeocaches",
                column: "GeocasheId",
                principalTable: "Geocashes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoundGeocaches_Persons_PersonId",
                table: "FoundGeocaches",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Geocashes_Persons_PersonId",
                table: "Geocashes",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
