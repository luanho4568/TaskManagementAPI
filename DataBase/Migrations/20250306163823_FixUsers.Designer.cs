﻿// <auto-generated />
using System;
using DataBase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataBase.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250306163823_FixUsers")]
    partial class FixUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("DataBase.Models.Users", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("Bio")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("HashPassword")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Last_login")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Lockout_EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Login_attempts")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Status_login")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("Updated_at")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
