﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PalletSyncApi.Context;

#nullable disable

namespace PalletSyncApi.Migrations
{
    [DbContext(typeof(PalletSyncDbContext))]
    partial class PalletSyncDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PalletSyncApi.Classes.Forklift", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastPalletId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LastPalletId");

                    b.HasIndex("LastUserId");

                    b.ToTable("Forklifts");

                    b.HasData(
                        new
                        {
                            Id = "F-0012",
                            LastPalletId = "P-0003",
                            LastUserId = "U-0001"
                        },
                        new
                        {
                            Id = "F-0007",
                            LastPalletId = "P-0003",
                            LastUserId = "U-0002"
                        },
                        new
                        {
                            Id = "F-0205",
                            LastPalletId = "P-0001",
                            LastUserId = "U-0003"
                        },
                        new
                        {
                            Id = "F-0016",
                            LastPalletId = "P-0005",
                            LastUserId = "U-0003"
                        });
                });

            modelBuilder.Entity("PalletSyncApi.Classes.Pallet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pallets");

                    b.HasData(
                        new
                        {
                            Id = "P-0001",
                            Location = "Warehouse A",
                            State = 2
                        },
                        new
                        {
                            Id = "P-0002",
                            Location = "Warehouse A",
                            State = 2
                        },
                        new
                        {
                            Id = "P-0003",
                            Location = "Warehouse A",
                            State = 0
                        },
                        new
                        {
                            Id = "P-0004",
                            Location = "Warehouse A",
                            State = 1
                        },
                        new
                        {
                            Id = "P-0005",
                            Location = "Warehouse A",
                            State = 3
                        });
                });

            modelBuilder.Entity("PalletSyncApi.Classes.PalletTrackingLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ForkliftId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PalletId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PalletLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PalletState")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ForkliftId");

                    b.HasIndex("PalletId");

                    b.HasIndex("UserId");

                    b.ToTable("PalletTrackingLog");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Action = "Forklift F-0012 placed pallet P-0001 on shelf S-0001 in Warehouse A by user U-0001",
                            DateTime = new DateTime(2024, 1, 21, 16, 3, 42, 638, DateTimeKind.Utc).AddTicks(8464),
                            ForkliftId = "F-0012",
                            PalletId = "P-0001",
                            PalletLocation = "Warehouse A",
                            PalletState = 2,
                            UserId = "U-0001"
                        },
                        new
                        {
                            Id = 2,
                            Action = "Forklift F-0007 placed pallet P-0002 on the floor in Warehouse B by user U-0002",
                            DateTime = new DateTime(2024, 1, 21, 16, 3, 42, 638, DateTimeKind.Utc).AddTicks(8467),
                            ForkliftId = "F-0007",
                            PalletId = "P-0002",
                            PalletLocation = "Warehouse B",
                            PalletState = 0,
                            UserId = "U-0002"
                        });
                });

            modelBuilder.Entity("PalletSyncApi.Classes.Shelf", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PalletId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PalletId")
                        .IsUnique()
                        .HasFilter("[PalletId] IS NOT NULL");

                    b.ToTable("Shelves");

                    b.HasData(
                        new
                        {
                            Id = "S-0001",
                            Location = "Warehouse A",
                            PalletId = "P-0001"
                        },
                        new
                        {
                            Id = "S-0002",
                            Location = "Warehouse A",
                            PalletId = "P-0002"
                        },
                        new
                        {
                            Id = "S-0003",
                            Location = "Warehouse A"
                        },
                        new
                        {
                            Id = "S-0004",
                            Location = "Warehouse A"
                        },
                        new
                        {
                            Id = "S-0005",
                            Location = "Warehouse A"
                        });
                });

            modelBuilder.Entity("PalletSyncApi.Classes.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ForkliftCertified")
                        .HasColumnType("bit");

                    b.Property<int>("IncorrectPalletPlacements")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Passcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "U-0001",
                            FirstName = "Kacper",
                            ForkliftCertified = true,
                            IncorrectPalletPlacements = 0,
                            LastName = "Wroblewski",
                            Passcode = "245tbgt",
                            UserType = 1
                        },
                        new
                        {
                            Id = "U-0002",
                            FirstName = "Nikita",
                            ForkliftCertified = true,
                            IncorrectPalletPlacements = 13,
                            LastName = "Fedans",
                            Passcode = "245tbgt",
                            UserType = 0
                        },
                        new
                        {
                            Id = "U-0003",
                            FirstName = "Teodor",
                            ForkliftCertified = true,
                            IncorrectPalletPlacements = 3,
                            LastName = "Donchev",
                            Passcode = "245tbgt",
                            UserType = 0
                        },
                        new
                        {
                            Id = "U-0004",
                            FirstName = "Vincent",
                            ForkliftCertified = false,
                            IncorrectPalletPlacements = 0,
                            LastName = "Arellano",
                            Passcode = "245tbgt",
                            UserType = 0
                        },
                        new
                        {
                            Id = "U-0005",
                            FirstName = "Kyle",
                            ForkliftCertified = false,
                            IncorrectPalletPlacements = 0,
                            LastName = "McQuillan",
                            Passcode = "245tbgt",
                            UserType = 0
                        },
                        new
                        {
                            Id = "U-0006",
                            FirstName = "Siya",
                            ForkliftCertified = false,
                            IncorrectPalletPlacements = 0,
                            LastName = "Salekar",
                            Passcode = "245tbgt",
                            UserType = 1
                        });
                });

            modelBuilder.Entity("PalletSyncApi.Classes.Forklift", b =>
                {
                    b.HasOne("PalletSyncApi.Classes.Pallet", "LastPallet")
                        .WithMany()
                        .HasForeignKey("LastPalletId");

                    b.HasOne("PalletSyncApi.Classes.User", "LastUser")
                        .WithMany()
                        .HasForeignKey("LastUserId");

                    b.Navigation("LastPallet");

                    b.Navigation("LastUser");
                });

            modelBuilder.Entity("PalletSyncApi.Classes.PalletTrackingLog", b =>
                {
                    b.HasOne("PalletSyncApi.Classes.Forklift", "Forklift")
                        .WithMany()
                        .HasForeignKey("ForkliftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PalletSyncApi.Classes.Pallet", "Pallet")
                        .WithMany()
                        .HasForeignKey("PalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PalletSyncApi.Classes.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forklift");

                    b.Navigation("Pallet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PalletSyncApi.Classes.Shelf", b =>
                {
                    b.HasOne("PalletSyncApi.Classes.Pallet", "Pallet")
                        .WithOne()
                        .HasForeignKey("PalletSyncApi.Classes.Shelf", "PalletId");

                    b.Navigation("Pallet");
                });
#pragma warning restore 612, 618
        }
    }
}
