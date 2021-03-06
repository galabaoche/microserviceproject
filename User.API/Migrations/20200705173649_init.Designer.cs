﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using User.API.Data;

namespace User.API.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20200705173649_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("User.API.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<byte>("Gender")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NameCard")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<string>("Tel")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("User.API.Models.BPFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("FormatFilePath")
                        .HasColumnType("text");

                    b.Property<string>("OriginFilePath")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserBPFiles");
                });

            modelBuilder.Entity("User.API.Models.UserProperty", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("AppUserId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("varchar(767)");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Key", "AppUserId", "Value");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserProperties");
                });

            modelBuilder.Entity("User.API.Models.UserTag", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("UserId", "Tag");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("User.API.Models.UserProperty", b =>
                {
                    b.HasOne("User.API.Models.AppUser", null)
                        .WithMany("Properties")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
