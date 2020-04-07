using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class TicketDatesRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "TicketsHistory",
                newName: "Finished");

            migrationBuilder.RenameColumn(
                name: "CheckOutDate",
                table: "TicketsHistory",
                newName: "CheckedOut");

            migrationBuilder.RenameColumn(
                name: "CheckInDate",
                table: "TicketsHistory",
                newName: "CheckedIn");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Finished",
                table: "TicketsHistory",
                newName: "FinishDate");

            migrationBuilder.RenameColumn(
                name: "CheckedOut",
                table: "TicketsHistory",
                newName: "CheckOutDate");

            migrationBuilder.RenameColumn(
                name: "CheckedIn",
                table: "TicketsHistory",
                newName: "CheckInDate");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Customers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
