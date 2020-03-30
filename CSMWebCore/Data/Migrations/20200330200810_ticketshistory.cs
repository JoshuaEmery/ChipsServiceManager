using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Data.Migrations
{
    public partial class ticketshistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TicketId = table.Column<int>(nullable: false),
                    TicketNumber = table.Column<int>(nullable: false),
                    DeviceId = table.Column<int>(nullable: false),
                    TicketStatus = table.Column<int>(nullable: false),
                    NeedsBackup = table.Column<bool>(nullable: false),
                    CheckedIn = table.Column<DateTime>(nullable: false),
                    Finished = table.Column<DateTime>(nullable: false),
                    CheckedOut = table.Column<DateTime>(nullable: false),
                    CheckInUserId = table.Column<string>(nullable: true),
                    CheckOutUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketsHistory");
        }
    }
}
