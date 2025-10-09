using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Formulario_soporte.Models;

public partial class FormularioDbContext : DbContext
{
    public FormularioDbContext()
    {
    }

    public FormularioDbContext(DbContextOptions<FormularioDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Sucursal> Sucursals { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       // => optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS; database=FormularioDB; User Id=sa;Password=123456789; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.IdEquipo).HasName("PK__equipo__EE01F88A8D30FC62");

            entity.ToTable("equipo");

            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.Activo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("activo");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modelo");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__equipo__id_usuar__3C69FB99");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK__reporte__87E4F5CB5F7747CD");

            entity.ToTable("reporte");

            entity.Property(e => e.IdReporte).HasColumnName("id_reporte");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaReporte).HasColumnName("fecha_reporte");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdUsuarioResponsable).HasColumnName("id_usuario_responsable");
            entity.Property(e => e.IdUsuarioTecnico).HasColumnName("id_usuario_tecnico");
            entity.Property(e => e.MantenimientoLogico)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("mantenimiento_logico");
            entity.Property(e => e.MantenimientoFisico)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("mantenimiento_fisico");
            entity.Property(e => e.MantenimientoReemplazo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("mantenimiento_reemplazo");
            entity.Property(e => e.TrabajoRealizado)
                .HasColumnType("text")
                .HasColumnName("trabajo_realizado");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reporte__id_equi__4222D4EF");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reporte__id_sucu__4316F928");

            entity.HasOne(d => d.IdUsuarioResponsableNavigation).WithMany(p => p.ReporteIdUsuarioResponsableNavigations)
                .HasForeignKey(d => d.IdUsuarioResponsable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reporte__id_usua__440B1D61");

            entity.HasOne(d => d.IdUsuarioTecnicoNavigation).WithMany(p => p.ReporteIdUsuarioTecnicoNavigations)
                .HasForeignKey(d => d.IdUsuarioTecnico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__reporte__id_usua__412EB0B6");
            entity.Property(e => e.Firma).HasColumnName("firma");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.IdSucursal).HasName("PK__sucursal__4C7580133F11B1ED");

            entity.ToTable("sucursal");

            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Empresa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("empresa");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__4E3E04AD2FB4B5B6");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
