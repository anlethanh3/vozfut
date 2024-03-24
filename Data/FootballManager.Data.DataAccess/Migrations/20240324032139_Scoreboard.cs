using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballManager.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Scoreboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "MatchDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "MatchDetails");
        }
    }
}
