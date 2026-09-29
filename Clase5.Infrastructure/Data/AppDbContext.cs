using Clase2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clase5.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)   // Actúa como un ORM, mapea nuestras entidades de Domain con la DB
{
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Profesor> Profesores => Set<Profesor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curso>(b =>
        {
            b.ToTable("Cursos");
            b.HasKey(c => c.Id);
            b.Property(c => c.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(200);
            b.Property(c => c.Credits)
                .HasColumnName("Credits")
                .IsRequired();
        });

        modelBuilder.Entity<Profesor>(b =>
        {
            b.ToTable("Profesores");
            b.HasKey(p => p.Id);
            b.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            b.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(100);
            b.Property(p => p.Legajo)
                .IsRequired()
                .HasMaxLength(20);
            b.HasIndex(p => p.Legajo)
                .IsUnique();
        });
    }
}