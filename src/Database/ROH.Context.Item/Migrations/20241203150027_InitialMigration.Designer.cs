﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ROH.Context.Item;

#nullable disable

namespace ROH.Context.Item.Migrations
{
    [DbContext(typeof(ItemContext))]
    [Migration("20241203150027_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ROH.Context.Item.Entities.Enchantment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Animation")
                        .HasColumnType("text");

                    b.Property<long?>("Damage")
                        .HasColumnType("bigint");

                    b.Property<long?>("Defense")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Enchantments");
                });

            modelBuilder.Entity("ROH.Context.Item.Entities.Item", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int?>("Attack")
                        .HasColumnType("integer");

                    b.Property<int?>("Defense")
                        .HasColumnType("integer");

                    b.Property<string>("Descricao")
                        .HasColumnType("text");

                    b.Property<string>("File")
                        .HasColumnType("text");

                    b.Property<string>("Format")
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Sprite")
                        .HasColumnType("text");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ROH.Context.Item.Entities.ItemEnchantment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdEnchantment")
                        .HasColumnType("bigint");

                    b.Property<long>("IdItem")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IdEnchantment");

                    b.HasIndex("IdItem");

                    b.ToTable("ItemEnchantments");
                });

            modelBuilder.Entity("ROH.Context.Item.Entities.ItemEnchantment", b =>
                {
                    b.HasOne("ROH.Context.Item.Entities.Enchantment", "Enchantment")
                        .WithMany("Items")
                        .HasForeignKey("IdEnchantment")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ROH.Context.Item.Entities.Item", "Item")
                        .WithMany("Enchantments")
                        .HasForeignKey("IdItem")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enchantment");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ROH.Context.Item.Entities.Enchantment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ROH.Context.Item.Entities.Item", b =>
                {
                    b.Navigation("Enchantments");
                });
#pragma warning restore 612, 618
        }
    }
}
