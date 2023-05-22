﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiCitasMedicas;

#nullable disable

namespace WebApiCitasMedicas.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230522061201_Modificaciones")]
    partial class Modificaciones
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Cita", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha_cita")
                        .HasColumnType("datetime2");

                    b.Property<int>("MedicoID")
                        .HasColumnType("int");

                    b.Property<int>("PacienteID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicoID");

                    b.HasIndex("PacienteID");

                    b.ToTable("Citas");
                });

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Medico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cedula")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre_med")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Medicos");
                });

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicoID")
                        .HasColumnType("int");

                    b.Property<float>("altura")
                        .HasColumnType("real");

                    b.Property<string>("hist_medico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("peso")
                        .HasColumnType("real");

                    b.Property<string>("sexo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MedicoID");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Cita", b =>
                {
                    b.HasOne("WebApiCitasMedicas.Entidades.Medico", "Medico")
                        .WithMany()
                        .HasForeignKey("MedicoID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApiCitasMedicas.Entidades.Paciente", "Paciente")
                        .WithMany()
                        .HasForeignKey("PacienteID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Medico");

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Paciente", b =>
                {
                    b.HasOne("WebApiCitasMedicas.Entidades.Medico", null)
                        .WithMany("pacientes")
                        .HasForeignKey("MedicoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Medico", b =>
                {
                    b.Navigation("pacientes");
                });
#pragma warning restore 612, 618
        }
    }
}
