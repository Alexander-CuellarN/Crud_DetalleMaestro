using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crud_DetalleMaestro.Models;

public partial class CrudMaestroDetalleContext : DbContext
{
    public CrudMaestroDetalleContext()
    {
    }

    public CrudMaestroDetalleContext(DbContextOptions<CrudMaestroDetalleContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategria).HasName("PK__categori__CCD3CE725C4F3DC3");

            entity.ToTable("categoria");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__DetalleP__48AFFD9583F7AAED");

            entity.ToTable("DetallePedido");

            entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .HasConstraintName("FK__DetallePe__IdPed__403A8C7D");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetallePe__IdPro__412EB0B6");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__9D335DC3DE58B12C");

            entity.ToTable("Pedido");

            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("total");

            entity.Property(e => e.Nombre)
                            .HasMaxLength(50)
                            .IsUnicode(false);

            entity.Property(e => e.Nit)
                            .HasMaxLength(50)
                            .IsUnicode(false);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__098892100D70128D");

            entity.ToTable("producto");

            entity.Property(e => e.Categoria).HasColumnName("categoria");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Precio");

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Categoria)
                .HasConstraintName("FK__producto__catego__3A81B327");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
