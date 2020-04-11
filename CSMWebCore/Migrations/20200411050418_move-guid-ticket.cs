using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class moveguidticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalId",
                table: "Tickets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d79d2551-0dd3-44ca-a3e7-56975d54897b", "1eab82ec-0919-4f06-b8c9-b1973666d738", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b5fe6107-7604-4674-8423-5479467c943e", "7a8f4c65-5bd0-4f6a-b187-7372da5786d3", "Technician", "TECHNICIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6fa13c67-406b-44cc-a46b-5bbcf6d2f335", "01e8d707-c915-4bc6-b1a5-2ef47efbd247", "ReadOnly", "READONLY" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6fa13c67-406b-44cc-a46b-5bbcf6d2f335");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b5fe6107-7604-4674-8423-5479467c943e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d79d2551-0dd3-44ca-a3e7-56975d54897b");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Tickets");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "895fb138-2d5d-4736-9056-2552f50e3934", "cbd59046-b764-4f52-a9f7-6b4942deb3b6", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cc22ccb7-b0e5-4fd0-9b53-b2a8d9fe733a", "bd254d5a-6beb-4411-86bd-ce378eba5e1a", "Technician", "TECHNICIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6ad5705b-f0bc-4beb-ae3c-d03c56cd8385", "48e04a5b-98b4-42d9-aa5a-0dae604f0bd8", "ReadOnly", "READONLY" });
        }
    }
}
