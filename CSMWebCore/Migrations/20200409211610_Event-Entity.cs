using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class EventEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f10201a-f7cf-48de-a276-7e2599e08c75");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "694f28e8-ef78-4d06-9b22-fcd375400489");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0950a4f-486f-4327-aae5-eb44d259e2ef");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4dc8bcd2-962f-4ea0-879c-ffa3d93125c1", "6a2caa4a-2eaf-461a-bfe7-341336103879", "ReadOnly", "READONLY" },
                    { "4ac3a91d-f82a-46db-9f8a-3fc704d4a5a2", "77fd5d1f-1cb5-4bdd-8609-801f8048a75e", "Administrator", "ADMINISTRATOR" },
                    { "9effe992-42ad-485d-8b54-7b64404f3257", "02ffc095-f615-4054-8013-dc8967e85bab", "Technician", "TECHNICIAN" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Category", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 23, 3, null, "Voicemail", 0m },
                    { 22, 3, null, "Phone Call", 0m },
                    { 21, 3, null, "Email", 0m },
                    { 20, 3, null, "In-Person", 0m },
                    { 19, 1, null, "Other", 100m },
                    { 18, 1, null, "Trackpad Replacement", 150m },
                    { 17, 1, null, "Storage Drive Replacement", 100m },
                    { 16, 1, null, "RAM Replacement", 100m },
                    { 15, 1, null, "Power Jack Replacement", 150m },
                    { 14, 1, null, "Keyboard Replacement", 125m },
                    { 13, 1, null, "Hinge Replacement", 150m },
                    { 11, 1, null, "Battery Replacement", 50m },
                    { 10, 2, null, "Other", 50m },
                    { 9, 2, null, "Virus/Malware Removal", 150m },
                    { 8, 2, null, "Program/Driver Installation", 50m },
                    { 7, 2, "Incremental updates to an OS where user data is preserved", "OS Updates", 50m },
                    { 6, 2, "Installation of an OS on a blank drive or over an old OS, writing over existing user data", "OS Installation", 150m },
                    { 5, 2, null, "Data Restore", 150m },
                    { 4, 2, null, "Data Backup", 150m },
                    { 3, 0, "Identification of the issue by inspecting device hardware and software", "Diagnostic", 50m },
                    { 2, 0, "Final event when a device returned to the customer", "Check-Out", 0m },
                    { 12, 1, null, "Display Replacement", 150m },
                    { 1, 0, "Initial event when a device is brought in and a ticket is opened", "Check-In", 0m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ac3a91d-f82a-46db-9f8a-3fc704d4a5a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dc8bcd2-962f-4ea0-879c-ffa3d93125c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9effe992-42ad-485d-8b54-7b64404f3257");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d0950a4f-486f-4327-aae5-eb44d259e2ef", "228be113-97b3-4419-b0a6-61b8a502f7b4", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5f10201a-f7cf-48de-a276-7e2599e08c75", "dd499839-ce45-4651-a4b8-98fa4800859d", "Technician", "TECHNICIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "694f28e8-ef78-4d06-9b22-fcd375400489", "05bd52ba-01c1-45c0-9422-93862247c074", "ReadOnly", "READONLY" });
        }
    }
}
