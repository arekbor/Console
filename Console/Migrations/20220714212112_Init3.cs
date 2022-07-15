using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Console.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ban",
                table: "DbUserContext");

            migrationBuilder.DropColumn(
                name: "Mute",
                table: "DbUserContext");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "DbUserContext");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ban",
                table: "DbUserContext",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Mute",
                table: "DbUserContext",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "DbUserContext",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
