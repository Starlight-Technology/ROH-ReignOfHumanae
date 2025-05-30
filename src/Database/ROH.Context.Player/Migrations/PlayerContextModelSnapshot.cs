﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ROH.Context.Player;

#nullable disable

namespace ROH.Context.Player.Migrations
{
    [DbContext(typeof(PlayerContext))]
    partial class PlayerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.AttackStatus", b =>
                {
                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("LongRangedWeaponLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("MagicWeaponLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("OneHandedWeaponLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("TwoHandedWeaponLevel")
                        .HasColumnType("bigint");

                    b.HasKey("IdCharacter");

                    b.ToTable("AttackStatuses");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Character", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("GuidAccount")
                        .HasColumnType("uuid");

                    b.Property<long?>("IdGuild")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdKingdom")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Race")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdGuild");

                    b.HasIndex("IdKingdom");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.CharacterInventory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("IdItem")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IdCharacter");

                    b.ToTable("CharacterInventory");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.CharacterSkill", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("IdSkill")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IdCharacter");

                    b.HasIndex("IdSkill");

                    b.ToTable("CharacterSkills");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.DefenseStatus", b =>
                {
                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("ArcaneDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("DarknessDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("EarthDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("FireDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("LightDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("LightningDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("MagicDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("PhysicDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("WaterDefenseLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("WindDefenseLevel")
                        .HasColumnType("bigint");

                    b.HasKey("IdCharacter");

                    b.ToTable("DefenseStatuses");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.EquippedItems", b =>
                {
                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdArmor")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdBoots")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdGloves")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdHead")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdLeftBracelet")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdLegs")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdNecklace")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdRightBracelet")
                        .HasColumnType("bigint");

                    b.HasKey("IdCharacter");

                    b.ToTable("EquippedItems");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.HandRing", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdEquippedItems")
                        .HasColumnType("bigint");

                    b.Property<long>("IdItem")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IdEquippedItems");

                    b.ToTable("RingsEquipped");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.PlayerPosition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdPlayer")
                        .HasColumnType("bigint");

                    b.Property<long>("PositionId")
                        .HasColumnType("bigint");

                    b.Property<long>("RotationId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IdPlayer")
                        .IsUnique();

                    b.HasIndex("PositionId");

                    b.HasIndex("RotationId");

                    b.ToTable("PlayersPosition");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Position", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdPlayer")
                        .HasColumnType("bigint");

                    b.Property<float>("X")
                        .HasColumnType("real");

                    b.Property<float>("Y")
                        .HasColumnType("real");

                    b.Property<float>("Z")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Rotation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<float>("W")
                        .HasColumnType("real");

                    b.Property<float>("X")
                        .HasColumnType("real");

                    b.Property<float>("Y")
                        .HasColumnType("real");

                    b.Property<float>("Z")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Rotations");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Skill", b =>
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

                    b.Property<long>("ManaCost")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Status", b =>
                {
                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("CurrentCarryWeight")
                        .HasColumnType("bigint");

                    b.Property<long>("CurrentHealth")
                        .HasColumnType("bigint");

                    b.Property<long>("CurrentMana")
                        .HasColumnType("bigint");

                    b.Property<long>("CurrentStamina")
                        .HasColumnType("bigint");

                    b.Property<long>("FullCarryWeight")
                        .HasColumnType("bigint");

                    b.Property<long>("FullHealth")
                        .HasColumnType("bigint");

                    b.Property<long>("FullMana")
                        .HasColumnType("bigint");

                    b.Property<long>("FullStamina")
                        .HasColumnType("bigint");

                    b.Property<long>("Level")
                        .HasColumnType("bigint");

                    b.Property<long>("MagicLevel")
                        .HasColumnType("bigint");

                    b.HasKey("IdCharacter");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Guilds.Guild", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Guilds.MembersPosition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("IdGuild")
                        .HasColumnType("bigint");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdCharacter")
                        .IsUnique();

                    b.HasIndex("IdGuild");

                    b.ToTable("MembersPositions");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.Champion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdCharacter")
                        .HasColumnType("bigint");

                    b.Property<long>("IdKingdom")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IdCharacter");

                    b.HasIndex("IdKingdom");

                    b.ToTable("Champions");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.Kingdom", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdRuler")
                        .HasColumnType("bigint");

                    b.Property<int>("Reign")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdRuler")
                        .IsUnique();

                    b.ToTable("Kingdoms");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.KingdomRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdKingdom")
                        .HasColumnType("bigint");

                    b.Property<long>("IdKingdom2")
                        .HasColumnType("bigint");

                    b.Property<int>("Situation")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdKingdom");

                    b.HasIndex("IdKingdom2");

                    b.ToTable("KingdomRelations");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.AttackStatus", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithOne("AttackStatus")
                        .HasForeignKey("ROH.Context.Player.Entities.Characters.AttackStatus", "IdCharacter");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Character", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Guilds.Guild", "Guild")
                        .WithMany("Characters")
                        .HasForeignKey("IdGuild");

                    b.HasOne("ROH.Context.Player.Entities.Kingdoms.Kingdom", "Kingdom")
                        .WithMany("Citizens")
                        .HasForeignKey("IdKingdom");

                    b.Navigation("Guild");

                    b.Navigation("Kingdom");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.CharacterInventory", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithMany("Inventory")
                        .HasForeignKey("IdCharacter");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.CharacterSkill", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithMany("Skills")
                        .HasForeignKey("IdCharacter");

                    b.HasOne("ROH.Context.Player.Entities.Characters.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("IdSkill")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.DefenseStatus", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithOne("DefenseStatus")
                        .HasForeignKey("ROH.Context.Player.Entities.Characters.DefenseStatus", "IdCharacter");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.EquippedItems", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithOne("EquippedItems")
                        .HasForeignKey("ROH.Context.Player.Entities.Characters.EquippedItems", "IdCharacter");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.HandRing", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.EquippedItems", "EquippedItems")
                        .WithMany("RightHandRings")
                        .HasForeignKey("IdEquippedItems")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EquippedItems");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.PlayerPosition", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Player")
                        .WithOne("PlayerPosition")
                        .HasForeignKey("ROH.Context.Player.Entities.Characters.PlayerPosition", "IdPlayer");

                    b.HasOne("ROH.Context.Player.Entities.Characters.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ROH.Context.Player.Entities.Characters.Rotation", "Rotation")
                        .WithMany()
                        .HasForeignKey("RotationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Position");

                    b.Navigation("Rotation");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Status", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithOne("Status")
                        .HasForeignKey("ROH.Context.Player.Entities.Characters.Status", "IdCharacter");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Guilds.MembersPosition", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithOne()
                        .HasForeignKey("ROH.Context.Player.Entities.Guilds.MembersPosition", "IdCharacter")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ROH.Context.Player.Entities.Guilds.Guild", "Guild")
                        .WithMany("MembersPositions")
                        .HasForeignKey("IdGuild")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.Champion", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Character")
                        .WithMany()
                        .HasForeignKey("IdCharacter")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ROH.Context.Player.Entities.Kingdoms.Kingdom", "Kingdom")
                        .WithMany("Champions")
                        .HasForeignKey("IdKingdom")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Kingdom");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.Kingdom", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Characters.Character", "Ruler")
                        .WithOne()
                        .HasForeignKey("ROH.Context.Player.Entities.Kingdoms.Kingdom", "IdRuler")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ruler");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.KingdomRelation", b =>
                {
                    b.HasOne("ROH.Context.Player.Entities.Kingdoms.Kingdom", "SourceKingdom")
                        .WithMany("OutgoingRelations")
                        .HasForeignKey("IdKingdom")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ROH.Context.Player.Entities.Kingdoms.Kingdom", "TargetKingdom")
                        .WithMany("IncomingRelations")
                        .HasForeignKey("IdKingdom2")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SourceKingdom");

                    b.Navigation("TargetKingdom");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.Character", b =>
                {
                    b.Navigation("AttackStatus");

                    b.Navigation("DefenseStatus");

                    b.Navigation("EquippedItems");

                    b.Navigation("Inventory");

                    b.Navigation("PlayerPosition");

                    b.Navigation("Skills");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Characters.EquippedItems", b =>
                {
                    b.Navigation("RightHandRings");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Guilds.Guild", b =>
                {
                    b.Navigation("Characters");

                    b.Navigation("MembersPositions");
                });

            modelBuilder.Entity("ROH.Context.Player.Entities.Kingdoms.Kingdom", b =>
                {
                    b.Navigation("Champions");

                    b.Navigation("Citizens");

                    b.Navigation("IncomingRelations");

                    b.Navigation("OutgoingRelations");
                });
#pragma warning restore 612, 618
        }
    }
}
