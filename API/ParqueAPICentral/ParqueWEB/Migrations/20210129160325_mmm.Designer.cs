﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParqueWEB.Data;

namespace ParqueWEB.Migrations
{
    [DbContext(typeof(ParqueWEBContext))]
    [Migration("20210129160325_mmm")]
    partial class mmm
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("ParqueWEB.Models.Lugar", b =>
                {
                    b.Property<long>("LugarID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("Fila")
                        .HasColumnType("int");

                    b.Property<long>("ParqueID")
                        .HasColumnType("bigint");

                    b.Property<float>("Preço")
                        .HasColumnType("real");

                    b.Property<int>("Sector")
                        .HasColumnType("int");

                    b.HasKey("LugarID");

                    b.HasIndex("ParqueID");

                    b.ToTable("Lugar");
                });

            modelBuilder.Entity("ParqueWEB.Models.Morada", b =>
                {
                    b.Property<long>("MoradaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("CodigoPostal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rua")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MoradaID");

                    b.ToTable("Morada");
                });

            modelBuilder.Entity("ParqueWEB.Models.Reserva", b =>
                {
                    b.Property<long>("ReservaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicio")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataReserva")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<long>("LugarID")
                        .HasColumnType("bigint");

                    b.HasKey("ReservaID");

                    b.HasIndex("LugarID");

                    b.ToTable("Reserva");
                });

            modelBuilder.Entity("ParqueWEB.Parque", b =>
                {
                    b.Property<long>("ParqueID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<int>("Lotacao")
                        .HasColumnType("int");

                    b.Property<long>("MoradaID")
                        .HasColumnType("bigint");

                    b.Property<string>("NomeParque")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParqueID");

                    b.HasIndex("MoradaID");

                    b.ToTable("Parque");
                });

            modelBuilder.Entity("ParqueWEB.Models.Lugar", b =>
                {
                    b.HasOne("ParqueWEB.Parque", "Parque")
                        .WithMany()
                        .HasForeignKey("ParqueID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parque");
                });

            modelBuilder.Entity("ParqueWEB.Models.Reserva", b =>
                {
                    b.HasOne("ParqueWEB.Models.Lugar", "Lugar")
                        .WithMany()
                        .HasForeignKey("LugarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lugar");
                });

            modelBuilder.Entity("ParqueWEB.Parque", b =>
                {
                    b.HasOne("ParqueWEB.Models.Morada", "Morada")
                        .WithMany()
                        .HasForeignKey("MoradaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Morada");
                });
#pragma warning restore 612, 618
        }
    }
}
