using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class removelogtypecontactmethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ContactMethod",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "LogType",
                table: "Logs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dba7019d-059f-4a88-ada4-d04b3365f0de", "3aa78b29-dc6c-447a-8266-c2241304625c", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "50f02611-772f-4aff-b096-7486fe9ab163", "72a65879-3645-427d-90ae-74ecf0303264", "Technician", "TECHNICIAN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6b89df29-2afc-482d-8ec1-72df3773f52e", "6734f14f-ca37-42a4-a8a3-37acc16b6317", "ReportReader", "REPORTREADER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50f02611-772f-4aff-b096-7486fe9ab163");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b89df29-2afc-482d-8ec1-72df3773f52e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dba7019d-059f-4a88-ada4-d04b3365f0de");

            migrationBuilder.AddColumn<int>(
                name: "ContactMethod",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LogType",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
