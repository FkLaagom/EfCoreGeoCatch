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
    [Migration("20190319140137_test5")]
    partial class test5
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
                    b.Property<int>("GeocasheId");

                    b.Property<int>("PersonId");

                    b.HasKey("GeocasheId", "PersonId");

                    b.HasIndex("PersonId");

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

            modelBuilder.Entity("Geocaching.Models.FoundGeocache", b =>
                {
                    b.HasOne("Geocaching.Models.Geocashe", "Geocashe")
                        .WithMany()
                        .HasForeignKey("GeocasheId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Geocaching.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Geocaching.Models.Geocashe", b =>
                {
                    b.HasOne("Geocaching.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonID");
                });
#pragma warning restore 612, 618
        }
    }
}
