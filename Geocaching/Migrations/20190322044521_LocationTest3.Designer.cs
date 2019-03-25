﻿// <auto-generated />
using System;
using Geocaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Geocaching.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190322044521_LocationTest3")]
    partial class LocationTest3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Geocaching.Models.FoundGeocache", b =>
                {
                    b.Property<int?>("GeocasheID");

                    b.Property<int?>("PersonID");

                    b.HasKey("GeocasheID", "PersonID");

                    b.HasIndex("PersonID");

                    b.ToTable("FoundGeocaches");
                });

            modelBuilder.Entity("Geocaching.Models.Geocashe", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int?>("PersonID");

                    b.HasKey("ID");

                    b.HasIndex("PersonID");

                    b.ToTable("Geocashes");
                });

            modelBuilder.Entity("Geocaching.Models.Person", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte>("StreetNumber");

                    b.HasKey("ID");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Geocaching.Models.Place", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("ID");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("Geocaching.Models.FoundGeocache", b =>
                {
                    b.HasOne("Geocaching.Models.Geocashe", "Geocashe")
                        .WithMany()
                        .HasForeignKey("GeocasheID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Geocaching.Models.Person", "Person")
                        .WithMany("FoundGeocaches")
                        .HasForeignKey("PersonID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Geocaching.Models.Geocashe", b =>
                {
                    b.HasOne("Geocaching.Models.Person", "Person")
                        .WithMany("Geocashes")
                        .HasForeignKey("PersonID");
                });

            modelBuilder.Entity("Geocaching.Models.Place", b =>
                {
                    b.OwnsOne("Microsoft.Maps.MapControl.WPF.Location", "Location", b1 =>
                        {
                            b1.Property<int>("PlaceID")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<double>("Altitude");

                            b1.Property<int>("AltitudeReference");

                            b1.Property<decimal>("Latitude")
                                .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                                .HasColumnName("Latitude")
                                .HasColumnType("NUMERIC(38, 16)");

                            b1.Property<decimal>("Longitude")
                                .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 38, scale: 17)))
                                .HasColumnName("Longitude")
                                .HasColumnType("NUMERIC(38, 16)");

                            b1.HasKey("PlaceID");

                            b1.ToTable("Places");

                            b1.HasOne("Geocaching.Models.Place")
                                .WithOne("Location")
                                .HasForeignKey("Microsoft.Maps.MapControl.WPF.Location", "PlaceID")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}