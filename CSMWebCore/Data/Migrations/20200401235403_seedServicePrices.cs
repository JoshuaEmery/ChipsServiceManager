using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Data.Migrations
{
    public partial class seedServicePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ServicePrices",
                columns: new[] { "Id", "Price", "Service" },
                values: new object[,]
                {
                    { 1, 0m, 0 },
                    { 16, 50m, 15 },
                    { 15, 150m, 14 },
                    { 14, 150m, 13 },
                    { 13, 150m, 12 },
                    { 12, 125m, 11 },
                    { 11, 150m, 10 },
                    { 10, 100m, 9 },
                    { 9, 100m, 8 },
                    { 8, 50m, 7 },
                    { 7, 150m, 6 },
                    { 6, 50m, 5 },
                    { 5, 150m, 4 },
                    { 4, 50m, 3 },
                    { 3, 0m, 2 },
                    { 2, 0m, 1 },
                    { 17, 150m, 16 },
                    { 18, 100m, 17 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ServicePrices",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
