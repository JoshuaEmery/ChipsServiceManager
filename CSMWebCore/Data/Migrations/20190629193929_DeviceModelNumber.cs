using Microsoft.EntityFrameworkCore.Migrations;

namespace CSMWebCore.Data.Migrations
{
    public partial class DeviceModelNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Devices",
                newName: "ModelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModelNumber",
                table: "Devices",
                newName: "Model");
        }
    }
}
