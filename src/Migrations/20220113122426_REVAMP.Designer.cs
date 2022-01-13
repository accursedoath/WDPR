﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace src.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220113122426_REVAMP")]
    partial class REVAMP
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("Aanmelding", b =>
                {
                    b.Property<int>("AanmeldingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Achternaam")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Leeftijdscategorie")
                        .HasColumnType("TEXT");

                    b.Property<string>("Stoornis")
                        .HasColumnType("TEXT");

                    b.Property<string>("Voornaam")
                        .HasColumnType("TEXT");

                    b.HasKey("AanmeldingId");

                    b.ToTable("Aanmeldingen");
                });

            modelBuilder.Entity("Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Achternaam")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Voornaam")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Woonplaats")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Woonplaats")
                        .IsUnique();

                    b.ToTable("Account");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Account");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruiker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicatieGebruiker");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Woonplaats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adres")
                        .HasColumnType("TEXT");

                    b.Property<string>("Postcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("plaats")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Woonplaats");
                });

            modelBuilder.Entity("src.Models.Bericht", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("TEXT");

                    b.Property<int?>("VerzenderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("VerzenderId");

                    b.ToTable("Berichten");
                });

            modelBuilder.Entity("Client", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<string>("ApplicatieGebruiker")
                        .HasColumnType("TEXT");

                    b.Property<bool>("magChatten")
                        .HasColumnType("INTEGER");

                    b.HasIndex("ApplicatieGebruiker")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("Client");
                });

            modelBuilder.Entity("Hulpverlener", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<string>("ApplicatieGebruiker")
                        .HasColumnType("TEXT")
                        .HasColumnName("Hulpverlener_ApplicatieGebruiker");

                    b.Property<string>("Beschrijving")
                        .HasColumnType("TEXT");

                    b.HasIndex("ApplicatieGebruiker")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("Hulpverlener");
                });

            modelBuilder.Entity("Moderator", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<string>("ApplicatieGebruiker")
                        .HasColumnType("TEXT")
                        .HasColumnName("Moderator_ApplicatieGebruiker");

                    b.HasIndex("ApplicatieGebruiker")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("Moderator");
                });

            modelBuilder.Entity("Voogd", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<string>("ApplicatieGebruiker")
                        .HasColumnType("TEXT")
                        .HasColumnName("Voogd_ApplicatieGebruiker");

                    b.HasIndex("ApplicatieGebruiker")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("Voogd");
                });

            modelBuilder.Entity("ApplicatieGebruiker", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.ApplicatieGebruiker");

                    b.HasDiscriminator().HasValue("ApplicatieGebruiker");
                });

            modelBuilder.Entity("Account", b =>
                {
                    b.HasOne("Woonplaats", "woonplaats")
                        .WithOne("account")
                        .HasForeignKey("Account", "Woonplaats");

                    b.Navigation("woonplaats");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.ApplicatieGebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.ApplicatieGebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.ApplicatieGebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.ApplicatieGebruikerToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.ApplicatieGebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Models.Bericht", b =>
                {
                    b.HasOne("Account", "Verzender")
                        .WithMany()
                        .HasForeignKey("VerzenderId");

                    b.Navigation("Verzender");
                });

            modelBuilder.Entity("Client", b =>
                {
                    b.HasOne("ApplicatieGebruiker", "User")
                        .WithOne("client")
                        .HasForeignKey("Client", "ApplicatieGebruiker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hulpverlener", b =>
                {
                    b.HasOne("ApplicatieGebruiker", "User")
                        .WithOne("hulpverlener")
                        .HasForeignKey("Hulpverlener", "ApplicatieGebruiker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Moderator", b =>
                {
                    b.HasOne("ApplicatieGebruiker", "User")
                        .WithOne("moderator")
                        .HasForeignKey("Moderator", "ApplicatieGebruiker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Voogd", b =>
                {
                    b.HasOne("ApplicatieGebruiker", "User")
                        .WithOne("voogd")
                        .HasForeignKey("Voogd", "ApplicatieGebruiker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Woonplaats", b =>
                {
                    b.Navigation("account");
                });

            modelBuilder.Entity("ApplicatieGebruiker", b =>
                {
                    b.Navigation("client");

                    b.Navigation("hulpverlener");

                    b.Navigation("moderator");

                    b.Navigation("voogd");
                });
#pragma warning restore 612, 618
        }
    }
}
