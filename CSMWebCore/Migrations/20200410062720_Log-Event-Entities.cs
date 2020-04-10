using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class LogEventEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24e5ed31-26ab-4c73-8eb1-a0ac89f93df0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a7b079d-cf02-4677-8265-59d47f7dc972");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ad335b4-3546-44a5-b0cb-fddc0a074c09");

            migrationBuilder.DropColumn(
                name: "Logged",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Logs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Logs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketStatus",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserCreated",
                table: "Logs",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "895fb138-2d5d-4736-9056-2552f50e3934", "cbd59046-b764-4f52-a9f7-6b4942deb3b6", "Administrator", "ADMINISTRATOR" },
                    { "cc22ccb7-b0e5-4fd0-9b53-b2a8d9fe733a", "bd254d5a-6beb-4411-86bd-ce378eba5e1a", "Technician", "TECHNICIAN" },
                    { "6ad5705b-f0bc-4beb-ae3c-d03c56cd8385", "48e04a5b-98b4-42d9-aa5a-0dae604f0bd8", "ReadOnly", "READONLY" }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { 1, "Final event when a device has been awaiting pickup for 30 days and is sent to Lost and Found", "Lost and Found", 0m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { 2, "Identification of the issue by inspecting device hardware and software", "Diagnostic", 50m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Name" },
                values: new object[] { 3, "Data Backup" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { 3, null, "Data Restore" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { 3, "Installation of an OS on a blank drive or over an old OS, writing over existing user data", "OS Installation", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { 3, "Incremental updates to an OS where user data is preserved", "OS Updates" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 3, "Program/Driver Installation", 50m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 3, "Virus/Malware Removal", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "Name" },
                values: new object[] { 3, "Other" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Battery Replacement", 50m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Category", "Name" },
                values: new object[] { 4, "Display Replacement" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Hinge Replacement", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Keyboard Replacement", 125m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Power Jack Replacement", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Category", "Name" },
                values: new object[] { 4, "RAM Replacement" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Storage Drive Replacement", 100m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Trackpad Replacement", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 4, "Other", 100m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Category", "Name" },
                values: new object[] { 5, "In-Person" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Category", "Name" },
                values: new object[] { 5, "Email" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Category", "Name" },
                values: new object[] { 5, "Phone Call" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Category", "Description", "Name", "Price" },
                values: new object[] { 24, 5, null, "Voicemail", 0m });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_EventId",
                table: "Logs",
                column: "EventId");

            migrationBuilder.Sql("UPDATE Logs SET EventId = 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Events_EventId",
                table: "Logs",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Events_EventId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_EventId",
                table: "Logs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ad5705b-f0bc-4beb-ae3c-d03c56cd8385");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "895fb138-2d5d-4736-9056-2552f50e3934");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc22ccb7-b0e5-4fd0-9b53-b2a8d9fe733a");

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "TicketStatus",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Logged",
                table: "Logs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ad335b4-3546-44a5-b0cb-fddc0a074c09", "ea012894-a128-4d2e-9758-d3b7062ea19c", "Administrator", "ADMINISTRATOR" },
                    { "24e5ed31-26ab-4c73-8eb1-a0ac89f93df0", "f86a0749-7875-409e-8f18-12ba5fc846f5", "Technician", "TECHNICIAN" },
                    { "5a7b079d-cf02-4677-8265-59d47f7dc972", "6a9bd444-9945-44bf-b772-fe252f3b7463", "ReadOnly", "READONLY" }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { 0, "Identification of the issue by inspecting device hardware and software", "Diagnostic", 50m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { 1, null, "Data Backup", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Name" },
                values: new object[] { 1, "Data Restore" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { 1, "Installation of an OS on a blank drive or over an old OS, writing over existing user data", "OS Installation" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { 1, "Incremental updates to an OS where user data is preserved", "OS Updates", 50m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Name" },
                values: new object[] { 1, null, "Program/Driver Installation" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 1, "Virus/Malware Removal", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 1, "Other", 50m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "Name" },
                values: new object[] { 2, "Battery Replacement" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 2, "Display Replacement", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Category", "Name" },
                values: new object[] { 2, "Hinge Replacement" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 2, "Keyboard Replacement", 125m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 2, "Power Jack Replacement", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 2, "RAM Replacement", 100m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Category", "Name" },
                values: new object[] { 2, "Storage Drive Replacement" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 2, "Trackpad Replacement", 150m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 2, "Other", 100m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Category", "Name", "Price" },
                values: new object[] { 3, "In-Person", 0m });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Category", "Name" },
                values: new object[] { 3, "Email" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Category", "Name" },
                values: new object[] { 3, "Phone Call" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Category", "Name" },
                values: new object[] { 3, "Voicemail" });
        }
    }
}
