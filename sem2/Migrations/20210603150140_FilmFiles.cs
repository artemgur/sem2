using Microsoft.EntityFrameworkCore.Migrations;

namespace sem2.Migrations
{
    public partial class FilmFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Films",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImagePath",
                table: "Films",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoImagePath",
                table: "Films",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoPath",
                table: "Films",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImagePath",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "LogoImagePath",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "VideoPath",
                table: "Films");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Films",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldDefaultValue: 0m);
        }
    }
}
