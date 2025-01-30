using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParafiaApk.Migrations
{
    /// <inheritdoc />
    public partial class migracja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdKsiadz",
                table: "Msza",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Msza_IdKsiadz",
                table: "Msza",
                column: "IdKsiadz");

            migrationBuilder.AddForeignKey(
                name: "FK_Msza_Ksiadz_IdKsiadz",
                table: "Msza",
                column: "IdKsiadz",
                principalTable: "Ksiadz",
                principalColumn: "IdKsiadz");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Msza_Ksiadz_IdKsiadz",
                table: "Msza");

            migrationBuilder.DropIndex(
                name: "IX_Msza_IdKsiadz",
                table: "Msza");

            migrationBuilder.DropColumn(
                name: "IdKsiadz",
                table: "Msza");
        }
    }
}
