﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Recommend.API.Data;

namespace Recommend.API.Migrations
{
    [DbContext(typeof(RecommendDbContext))]
    [Migration("20200716084032_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Recommend.API.Models.ProjectRecommend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<int>("ReCommendType")
                        .HasColumnType("int");

                    b.Property<string>("FinStage")
                        .HasColumnType("text");

                    b.Property<string>("FromUserAvatar")
                        .HasColumnType("text");

                    b.Property<int>("FromUserId")
                        .HasColumnType("int");

                    b.Property<string>("FromUserName")
                        .HasColumnType("text");

                    b.Property<string>("Introduction")
                        .HasColumnType("text");

                    b.Property<string>("ProjectAvatar")
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReCommendTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Tags")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProjectRecommends");
                });

            modelBuilder.Entity("Recommend.API.Models.ProjectReferenceUser", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectRecommendId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id", "UserId");

                    b.HasIndex("ProjectRecommendId");

                    b.ToTable("ProjectReferenceUsers");
                });

            modelBuilder.Entity("Recommend.API.Models.ProjectReferenceUser", b =>
                {
                    b.HasOne("Recommend.API.Models.ProjectRecommend", null)
                        .WithMany("ProjectReferenceUsers")
                        .HasForeignKey("ProjectRecommendId");
                });
#pragma warning restore 612, 618
        }
    }
}
