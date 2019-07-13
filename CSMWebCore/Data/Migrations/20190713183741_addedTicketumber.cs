using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Data.Migrations
{
    public partial class addedTicketumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketNumber",
                table: "Tickets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "Tickets");
        }
    }
}
