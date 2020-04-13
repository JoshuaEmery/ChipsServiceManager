using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class TicketStatusRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "TicketStatus",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tickets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28117bd6-78b6-4c1e-9a52-2a4cd350b321", "2c2313d3-daf4-4393-90c0-dd6661a0b8c1", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "19825252-02e7-4298-9d80-eb5f39403a48", "0461274a-c477-4314-8b8a-4f9a08d22e2e", "Technician", "TECHNICIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "397a34f4-a4d5-4a59-bb6a-e3b656250b39", "7faf0496-d7e0-4505-8b8b-4f88b56ad7e2", "ReadOnly", "READONLY" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19825252-02e7-4298-9d80-eb5f39403a48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28117bd6-78b6-4c1e-9a52-2a4cd350b321");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "397a34f4-a4d5-4a59-bb6a-e3b656250b39");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketStatus",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
