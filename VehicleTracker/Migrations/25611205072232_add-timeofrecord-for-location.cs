using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleTracker.Migrations
{
    public partial class addtimeofrecordforlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "Id",
                keyValue: new Guid("084c0323-b80a-484c-99a1-286462ffea03"));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfRecord",
                table: "Location",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "Latitude", "Longitude", "TimeOfRecord", "VehicleId" },
                values: new object[] { new Guid("0e5cc4b8-4c97-4f8a-b0ef-46d76aeb1e98"), 100m, 200m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("62d65fe9-35d2-4529-983e-d8f92441dfd8") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "Id",
                keyValue: new Guid("0e5cc4b8-4c97-4f8a-b0ef-46d76aeb1e98"));

            migrationBuilder.DropColumn(
                name: "TimeOfRecord",
                table: "Location");

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "Latitude", "Longitude", "VehicleId" },
                values: new object[] { new Guid("084c0323-b80a-484c-99a1-286462ffea03"), 100m, 200m, new Guid("62d65fe9-35d2-4529-983e-d8f92441dfd8") });
        }
    }
}
