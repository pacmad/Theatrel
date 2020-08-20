﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using theatrel.DataAccess;

namespace theatrel.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200818134440_remove_locale_from_subscription")]
    partial class remove_locale_from_subscription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("theatrel.DataAccess.Entities.ChatInfoEntity", b =>
                {
                    b.Property<long>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Culture")
                        .HasColumnType("text");

                    b.Property<int>("CurrentStepId")
                        .HasColumnType("integer");

                    b.Property<string>("DbDays")
                        .HasColumnType("text");

                    b.Property<string>("DbTypes")
                        .HasColumnType("text");

                    b.Property<int>("DialogState")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastMessage")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PreviousStepId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("When")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ChatId");

                    b.ToTable("TlChats");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.PerformanceChangeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MinPrice")
                        .HasColumnType("integer");

                    b.Property<int>("PerformanceDataId")
                        .HasColumnType("integer");

                    b.Property<int?>("PerformanceEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("ReasonOfChanges")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PerformanceEntityId");

                    b.ToTable("PerformanceChanges");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.PerformanceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<int>("MinPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Performances");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.PerformanceFilterEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("DbDaysOfWeek")
                        .HasColumnType("text");

                    b.Property<string>("DbLocations")
                        .HasColumnType("text");

                    b.Property<string>("DbPerformanceTypes")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PartOfDay")
                        .HasColumnType("integer");

                    b.Property<int>("PerformanceId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Filters");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.SubscriptionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PerformanceFilterId")
                        .HasColumnType("integer");

                    b.Property<long>("TelegramUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("TrackingChanges")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PerformanceFilterId");

                    b.HasIndex("TelegramUserId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.TelegramUserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Culture")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TlUsers");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.PerformanceChangeEntity", b =>
                {
                    b.HasOne("theatrel.DataAccess.Entities.PerformanceEntity", "PerformanceEntity")
                        .WithMany("Changes")
                        .HasForeignKey("PerformanceEntityId");
                });

            modelBuilder.Entity("theatrel.DataAccess.Entities.SubscriptionEntity", b =>
                {
                    b.HasOne("theatrel.DataAccess.Entities.PerformanceFilterEntity", "PerformanceFilter")
                        .WithMany()
                        .HasForeignKey("PerformanceFilterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("theatrel.DataAccess.Entities.TelegramUserEntity", "TelegramUser")
                        .WithMany()
                        .HasForeignKey("TelegramUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
