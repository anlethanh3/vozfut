using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballManager.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MemberStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Finishing",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Passing",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Skill",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stamina",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finishing",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Passing",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RealName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Skill",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Stamina",
                table: "Members");
        }
    }
}
