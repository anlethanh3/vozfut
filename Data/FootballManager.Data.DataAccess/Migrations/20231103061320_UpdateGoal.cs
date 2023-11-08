using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballManager.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Assist",
                table: "MatchDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Goal",
                table: "MatchDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assist",
                table: "MatchDetails");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "MatchDetails");
        }
    }
}
