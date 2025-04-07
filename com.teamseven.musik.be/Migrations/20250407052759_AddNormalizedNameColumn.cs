using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.teamseven.musik.be.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Track",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Podcast",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Playlist",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Genre",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Artist",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Album",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Podcast");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Playlist");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Artist");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Album");
        }
    }
}
