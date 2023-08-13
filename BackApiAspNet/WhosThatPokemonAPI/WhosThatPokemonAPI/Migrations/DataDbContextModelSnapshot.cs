﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WhosThatPokemonAPI.Data;

#nullable disable

namespace WhosThatPokemonAPI.Migrations
{
    [DbContext(typeof(DataDbContext))]
    partial class DataDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PokemonType", b =>
                {
                    b.Property<int>("PokemonsId")
                        .HasColumnType("int");

                    b.Property<int>("TypesId")
                        .HasColumnType("int");

                    b.HasKey("PokemonsId", "TypesId");

                    b.HasIndex("TypesId");

                    b.ToTable("PokemonType");
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.Pokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("picture");

                    b.HasKey("Id");

                    b.ToTable("Pokemon");
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("Type");
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit")
                        .HasColumnName("isAdmin");

                    b.Property<int>("Level")
                        .HasColumnType("int")
                        .HasColumnName("level");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("pseudo");

                    b.Property<int>("Xp")
                        .HasColumnType("int")
                        .HasColumnName("xp");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.UserPokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsWin")
                        .HasColumnType("bit");

                    b.Property<int>("PokemonId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PokemonId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPokemon");
                });

            modelBuilder.Entity("PokemonType", b =>
                {
                    b.HasOne("WhosThatPokemonAPI.Models.Pokemon", null)
                        .WithMany()
                        .HasForeignKey("PokemonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhosThatPokemonAPI.Models.Type", null)
                        .WithMany()
                        .HasForeignKey("TypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.UserPokemon", b =>
                {
                    b.HasOne("WhosThatPokemonAPI.Models.Pokemon", "Pokemon")
                        .WithMany("Users")
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhosThatPokemonAPI.Models.User", "User")
                        .WithMany("Pokemons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pokemon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.Pokemon", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("WhosThatPokemonAPI.Models.User", b =>
                {
                    b.Navigation("Pokemons");
                });
#pragma warning restore 612, 618
        }
    }
}