using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleTracker.Migrations
{
    public partial class addvehicleidforlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Vehicle_VehicleId",
                table: "Location");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "Location",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("62d65fe9-35d2-4529-983e-d8f92441dfd8"), "Mercedes" });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "Latitude", "Longitude", "VehicleId" },
                values: new object[] { new Guid("084c0323-b80a-484c-99a1-286462ffea03"), 100m, 200m, new Guid("62d65fe9-35d2-4529-983e-d8f92441dfd8") });

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Vehicle_VehicleId",
                table: "Location",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Vehicle_VehicleId",
                table: "Location");

            migrationBuilder.DeleteData(
                table: "Location",
                keyColumn: "Id",
                keyValue: new Guid("084c0323-b80a-484c-99a1-286462ffea03"));

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "Id",
                keyValue: new Guid("62d65fe9-35d2-4529-983e-d8f92441dfd8"));

            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "Location",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Vehicle_VehicleId",
                table: "Location",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
