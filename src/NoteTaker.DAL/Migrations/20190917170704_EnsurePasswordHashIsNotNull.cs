using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteTaker.DAL.Migrations
{
    public partial class EnsurePasswordHashIsNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "User",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
