using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParafiaApk.Migrations
{
    /// <inheritdoc />
    public partial class DodanieStatusDoSakrament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Sakrament",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sakrament");
        }
    }
}
