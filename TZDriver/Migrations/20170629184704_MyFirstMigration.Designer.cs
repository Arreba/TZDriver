using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TZDriver.DataStores;
using TZDriver.Models.Tools.Utilities;

namespace TZDriver.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170629184704_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("TZDriver.Models.DataModels.FuelData", b =>
                {
                    b.Property<int>("FuelDataId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OfficeRepName");

                    b.Property<string>("RefillCost");

                    b.Property<DateTime>("RefillDateTime");

                    b.Property<double>("RefillLatitude");

                    b.Property<double>("RefillLongitude");

                    b.Property<string>("RefillStation");

                    b.Property<double>("RefillVolume");

                    b.Property<byte[]>("VehicleAfterPicture");

                    b.Property<int>("VehicleAssetType");

                    b.Property<byte[]>("VehicleBeforePicture");

                    b.Property<string>("VehicleNumber");

                    b.Property<string>("VehicleOdometer");

                    b.HasKey("FuelDataId");

                    b.ToTable("FuelDatas");
                });

            modelBuilder.Entity("TZDriver.Models.DataModels.TripData", b =>
                {
                    b.Property<int>("TripDataId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AmountPaid");

                    b.Property<string>("CustomerName");

                    b.Property<string>("CustomerPhone1");

                    b.Property<string>("CustomerPhone2");

                    b.Property<bool>("IsPaymentComplete");

                    b.Property<bool>("IsTripComplete");

                    b.Property<bool>("IsTripStarted");

                    b.Property<int>("Mode");

                    b.Property<int>("Pay");

                    b.Property<string>("TripEndAddress");

                    b.Property<string>("TripEndArea");

                    b.Property<DateTime>("TripEndTime");

                    b.Property<string>("TripStartAddress");

                    b.Property<string>("TripStartArea");

                    b.Property<double?>("TripStartLatitude");

                    b.Property<double?>("TripStartLongitude");

                    b.Property<DateTime>("TripStartTime");

                    b.Property<int>("TripType");

                    b.Property<int>("TripUser");

                    b.Property<string>("VehicleNumber");

                    b.HasKey("TripDataId");

                    b.ToTable("TripDatas");
                });

            modelBuilder.Entity("TZDriver.Models.DataModels.TripPoint", b =>
                {
                    b.Property<int>("TripPointId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("RecordedTimeStamp");

                    b.Property<int>("TripDataId");

                    b.Property<double>("TripHeading");

                    b.Property<double>("TripLatitude");

                    b.Property<double>("TripLongitude");

                    b.Property<double>("TripSpeed");

                    b.HasKey("TripPointId");

                    b.HasIndex("TripDataId");

                    b.ToTable("TripPoints");
                });

            modelBuilder.Entity("TZDriver.Models.DataModels.TripPoint", b =>
                {
                    b.HasOne("TZDriver.Models.DataModels.TripData", "TripData")
                        .WithMany("Points")
                        .HasForeignKey("TripDataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
