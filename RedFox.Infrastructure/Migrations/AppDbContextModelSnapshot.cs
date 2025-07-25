﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RedFox.Infrastructure;

#nullable disable

namespace RedFox.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.7");

            modelBuilder.Entity("RedFox.Domain.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GeoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("street")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("suite")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("zipcode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GeoId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("RedFox.Domain.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bs")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CatchPhrase")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("RedFox.Domain.Entities.Geo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Lat")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Lng")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Geos");
                });

            modelBuilder.Entity("RedFox.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AddressId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("CompanyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RedFox.Domain.Entities.Address", b =>
                {
                    b.HasOne("RedFox.Domain.Entities.Geo", "Geo")
                        .WithOne()
                        .HasForeignKey("RedFox.Domain.Entities.Address", "GeoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Geo");
                });

            modelBuilder.Entity("RedFox.Domain.Entities.User", b =>
                {
                    b.HasOne("RedFox.Domain.Entities.Address", "Address")
                        .WithOne()
                        .HasForeignKey("RedFox.Domain.Entities.User", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RedFox.Domain.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("RedFox.Domain.Entities.Company", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
