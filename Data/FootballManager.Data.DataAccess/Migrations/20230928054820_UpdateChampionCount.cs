using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballManager.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChampionCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChampionCount",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChampionCount",
                table: "Members");
        }
    }
}
