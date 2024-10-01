﻿// <auto-generated />
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Vaultex.Configuration;

#nullable disable

namespace Vaultex.Configuration.Migrations
{
    [DbContext(typeof(ConfigContext))]
    partial class ConfigContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Vaultex.Configuration.Models.SettingDbo", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BaseType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("SubType");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<JsonDocument>("JsonConfig")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<Guid?>("ParentUuid")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("ParentUuid");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Vaultex.Configuration.Models.SettingDbo", b =>
                {
                    b.HasOne("Vaultex.Configuration.Models.SettingDbo", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentUuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Vaultex.Configuration.Models.SettingDbo", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
