using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Console.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "DbUserContext");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ban",
                table: "DbUserContext");

            migrationBuilder.DropColumn(
                name: "Mute",
                table: "DbUserContext");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "DbUserContext",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
