﻿// <auto-generated />
using System;
using DockerMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DockerMvc.Migrations
{
    [DbContext(typeof(BaseDbContext))]
    [Migration("20240712025929_Actualizado")]
    partial class Actualizado
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DockerMvc.Models.Categoria", b =>
                {
                    b.Property<int>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CatId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("DockerMvc.Models.Permission", b =>
                {
                    b.Property<int>("PermId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PermName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PermId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DockerMvc.Models.Productos", b =>
                {
                    b.Property<int>("ProduId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Imagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProduId");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("DockerMvc.Models.Profile", b =>
                {
                    b.Property<int>("ProId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProEncryptedPass")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProEstado")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("ProImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProLastname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("DockerMvc.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DockerMvc.Models.RolePermission", b =>
                {
                    b.Property<int>("RolePermId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PermId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("RolePermId");

                    b.HasIndex("PermId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("DockerMvc.Models.RoleProfile", b =>
                {
                    b.Property<int>("ProRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("ProRoleId");

                    b.HasIndex("ProId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleProfiles");
                });

            modelBuilder.Entity("DockerMvc.Models.SubCategoria", b =>
                {
                    b.Property<int>("SubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatId")
                        .HasColumnType("int");

                    b.Property<int>("ProduId")
                        .HasColumnType("int");

                    b.Property<int?>("RoleCatId")
                        .HasColumnType("int");

                    b.HasKey("SubId");

                    b.HasIndex("ProduId");

                    b.HasIndex("RoleCatId");

                    b.ToTable("SubCategorias");
                });

            modelBuilder.Entity("DockerMvc.Models.RolePermission", b =>
                {
                    b.HasOne("DockerMvc.Models.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DockerMvc.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DockerMvc.Models.RoleProfile", b =>
                {
                    b.HasOne("DockerMvc.Models.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DockerMvc.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DockerMvc.Models.SubCategoria", b =>
                {
                    b.HasOne("DockerMvc.Models.Productos", "Productos")
                        .WithMany()
                        .HasForeignKey("ProduId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DockerMvc.Models.Categoria", "Role")
                        .WithMany()
                        .HasForeignKey("RoleCatId");

                    b.Navigation("Productos");

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
