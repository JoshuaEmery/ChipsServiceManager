using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Data.Migrations
{
    public partial class onetomanycomplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.Id);
                });

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
                    CheckOutUserId = table.Column<string>(nullable: true),
                    AddedToHistory = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Updates_TicketId",
                table: "Updates",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DeviceId",
                table: "Tickets",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_TicketId",
                table: "Logs",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Tickets_TicketId",
                table: "Logs",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Devices_DeviceId",
                table: "Tickets",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Updates_Tickets_TicketId",
                table: "Updates",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Tickets_TicketId",
                table: "Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Devices_DeviceId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Updates_Tickets_TicketId",
                table: "Updates");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "TicketsHistory");

            migrationBuilder.DropIndex(
                name: "IX_Updates_TicketId",
                table: "Updates");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_DeviceId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Logs_TicketId",
                table: "Logs");
        }
    }
}
