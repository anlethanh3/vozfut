using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballManager.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TeamRivalsCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasTeamRival",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TeamRivals",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTeamRival",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamRivals",
                table: "Matches");
        }
    }
}
