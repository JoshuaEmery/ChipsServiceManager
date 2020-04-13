using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Migrations
{
    public partial class RenameOtherEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "fc4e2ae9-5a03-4706-9b0e-d4e5c504d444", "8d4e8ff9-9bbe-4cbd-8605-c9fb6cfb8ad3", "Administrator", "ADMINISTRATOR" },
                    { "0c0deb4c-0a86-472f-a063-70370d8d5e08", "e07fd86e-849a-4eaf-bd50-ddecb3bdcdfe", "Technician", "TECHNICIAN" },
                    { "9da6a21e-423d-4322-990d-7a633ece08d6", "2ce71a52-c44e-4f5b-94d3-b541316f19a3", "ReportReader", "REPORTREADER" }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Misc. Software");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "Misc. Hardware");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                values: new object[,]
                {
                    { "28117bd6-78b6-4c1e-9a52-2a4cd350b321", "2c2313d3-daf4-4393-90c0-dd6661a0b8c1", "Administrator", "ADMINISTRATOR" },
                    { "19825252-02e7-4298-9d80-eb5f39403a48", "0461274a-c477-4314-8b8a-4f9a08d22e2e", "Technician", "TECHNICIAN" },
                    { "397a34f4-a4d5-4a59-bb6a-e3b656250b39", "7faf0496-d7e0-4505-8b8b-4f88b56ad7e2", "ReadOnly", "READONLY" }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Other");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "Other");
        }
    }
}
