using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Data.Migrations
{
    public partial class RenameGetContactMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GetContactMethod",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "ContactMethod",
                table: "Logs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactMethod",
                table: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "GetContactMethod",
                table: "Logs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
