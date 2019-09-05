using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteTaker.DAL.Migrations
{
    public partial class AddIndexToCreatedAndModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Note_Created",
                table: "Note",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Note_Modified",
                table: "Note",
                column: "Modified");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Note_Created",
                table: "Note");

            migrationBuilder.DropIndex(
                name: "IX_Note_Modified",
                table: "Note");
        }
    }
}
