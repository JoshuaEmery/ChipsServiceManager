using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class cleanupunusedtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicePrices");

            migrationBuilder.DropTable(
                name: "TicketsHistory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c0deb4c-0a86-472f-a063-70370d8d5e08");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9da6a21e-423d-4322-990d-7a633ece08d6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc4e2ae9-5a03-4706-9b0e-d4e5c504d444");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4b923782-3c18-4cc5-aaab-fd64b17a605a", "a4140cd9-87cf-4f33-a73b-c52f823838b9", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35f0bf98-a7c6-4a06-9652-94eae1ce57db", "459960b7-140a-47d1-b5ec-25a4c0170134", "Technician", "TECHNICIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d156472a-cf1a-463a-abf7-3a7301dd19e8", "f2c3a074-b1dc-408a-9ac1-d548a1351a9d", "ReportReader", "REPORTREADER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35f0bf98-a7c6-4a06-9652-94eae1ce57db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b923782-3c18-4cc5-aaab-fd64b17a605a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d156472a-cf1a-463a-abf7-3a7301dd19e8");

            migrationBuilder.CreateTable(
                name: "ServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Service = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedToHistory = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckOutUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckedOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NeedsBackup = table.Column<bool>(type: "bit", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    TicketNumber = table.Column<int>(type: "int", nullable: false),
                    TicketStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsHistory", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9da6a21e-423d-4322-990d-7a633ece08d6", "2ce71a52-c44e-4f5b-94d3-b541316f19a3", "ReportReader", "REPORTREADER" },
                    { "fc4e2ae9-5a03-4706-9b0e-d4e5c504d444", "8d4e8ff9-9bbe-4cbd-8605-c9fb6cfb8ad3", "Administrator", "ADMINISTRATOR" },
                    { "0c0deb4c-0a86-472f-a063-70370d8d5e08", "e07fd86e-849a-4eaf-bd50-ddecb3bdcdfe", "Technician", "TECHNICIAN" }
                });

            migrationBuilder.InsertData(
                table: "ServicePrices",
                columns: new[] { "Id", "Price", "Service" },
                values: new object[,]
                {
                    { 18, 100m, 17 },
                    { 17, 150m, 16 },
                    { 16, 50m, 15 },
                    { 15, 150m, 14 },
                    { 14, 150m, 13 },
                    { 13, 150m, 12 },
                    { 12, 125m, 11 },
                    { 11, 150m, 10 },
                    { 9, 100m, 8 },
                    { 8, 50m, 7 },
                    { 7, 150m, 6 },
                    { 6, 50m, 5 },
                    { 5, 150m, 4 },
                    { 4, 50m, 3 },
                    { 3, 0m, 2 },
                    { 2, 0m, 1 },
                    { 10, 100m, 9 },
                    { 1, 0m, 0 }
                });
        }
    }
}
