using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TZDriver.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelDatas",
                columns: table => new
                {
                    FuelDataId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OfficeRepName = table.Column<string>(nullable: true),
                    RefillCost = table.Column<string>(nullable: true),
                    RefillDateTime = table.Column<DateTime>(nullable: false),
                    RefillLatitude = table.Column<double>(nullable: false),
                    RefillLongitude = table.Column<double>(nullable: false),
                    RefillStation = table.Column<string>(nullable: true),
                    RefillVolume = table.Column<double>(nullable: false),
                    VehicleAfterPicture = table.Column<byte[]>(nullable: true),
                    VehicleAssetType = table.Column<int>(nullable: false),
                    VehicleBeforePicture = table.Column<byte[]>(nullable: true),
                    VehicleNumber = table.Column<string>(nullable: true),
                    VehicleOdometer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelDatas", x => x.FuelDataId);
                });

            migrationBuilder.CreateTable(
                name: "TripDatas",
                columns: table => new
                {
                    TripDataId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmountPaid = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerPhone1 = table.Column<string>(nullable: true),
                    CustomerPhone2 = table.Column<string>(nullable: true),
                    IsPaymentComplete = table.Column<bool>(nullable: false),
                    IsTripComplete = table.Column<bool>(nullable: false),
                    IsTripStarted = table.Column<bool>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Pay = table.Column<int>(nullable: false),
                    TripEndAddress = table.Column<string>(nullable: true),
                    TripEndArea = table.Column<string>(nullable: true),
                    TripEndTime = table.Column<DateTime>(nullable: false),
                    TripStartAddress = table.Column<string>(nullable: true),
                    TripStartArea = table.Column<string>(nullable: true),
                    TripStartLatitude = table.Column<double>(nullable: true),
                    TripStartLongitude = table.Column<double>(nullable: true),
                    TripStartTime = table.Column<DateTime>(nullable: false),
                    TripType = table.Column<int>(nullable: false),
                    TripUser = table.Column<int>(nullable: false),
                    VehicleNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripDatas", x => x.TripDataId);
                });

            migrationBuilder.CreateTable(
                name: "TripPoints",
                columns: table => new
                {
                    TripPointId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecordedTimeStamp = table.Column<DateTime>(nullable: false),
                    TripDataId = table.Column<int>(nullable: false),
                    TripHeading = table.Column<double>(nullable: false),
                    TripLatitude = table.Column<double>(nullable: false),
                    TripLongitude = table.Column<double>(nullable: false),
                    TripSpeed = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPoints", x => x.TripPointId);
                    table.ForeignKey(
                        name: "FK_TripPoints_TripDatas_TripDataId",
                        column: x => x.TripDataId,
                        principalTable: "TripDatas",
                        principalColumn: "TripDataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripPoints_TripDataId",
                table: "TripPoints",
                column: "TripDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelDatas");

            migrationBuilder.DropTable(
                name: "TripPoints");

            migrationBuilder.DropTable(
                name: "TripDatas");
        }
    }
}
