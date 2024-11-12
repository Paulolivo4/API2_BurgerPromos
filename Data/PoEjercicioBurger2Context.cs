using System;
using System.Collections.Generic;
using API2.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API2.Data;

public partial class PoEjercicioBurger2Context : DbContext
{
    public PoEjercicioBurger2Context()
    {
    }

    public PoEjercicioBurger2Context(DbContextOptions<PoEjercicioBurger2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Burger> Burgers { get; set; }

    public virtual DbSet<Promo> Promos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=PO_EjercicioBurger2");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Burger>(entity =>
        {
            entity.ToTable("Burger");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceCeiling).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Promo>(entity =>
        {
            entity.ToTable("Promo");

            entity.HasIndex(e => e.BurgerId, "IX_Promo_BurgerId");

            entity.HasOne(d => d.Burger).WithMany(p => p.Promos).HasForeignKey(d => d.BurgerId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
