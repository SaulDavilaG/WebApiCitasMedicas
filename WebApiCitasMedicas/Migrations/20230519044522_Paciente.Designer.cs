﻿// <auto-generated />
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
    [Migration("20230519044522_Paciente")]
    partial class Paciente
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Medico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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

            modelBuilder.Entity("WebApiCitasMedicas.Entidades.Paciente", b =>
                {
                    b.HasOne("WebApiCitasMedicas.Entidades.Medico", "Medico")
                        .WithMany()
                        .HasForeignKey("MedicoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medico");
                });
#pragma warning restore 612, 618
        }
    }
}
