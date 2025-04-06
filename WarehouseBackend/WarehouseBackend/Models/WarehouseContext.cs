using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WarehouseBackend.Models;

public partial class WarehouseContext : DbContext
{
    
    public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

     public virtual DbSet<Transaction> Transactions { get; set; }

 


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PRIMARY");

            entity.ToTable("inventory");

            entity.HasIndex(e => e.LocationId, "LocationId");

            entity.HasIndex(e => e.MaterialId, "MaterialNumber");

            entity.Property(e => e.InventoryId)
                .HasColumnType("int(11)")
                .HasColumnName("InventoryID");
            entity.Property(e => e.LocationId)
                .HasColumnType("int(11)")
                .HasColumnName("LocationID");
            entity.Property(e => e.MaterialId)
                .HasColumnType("int(11)")
                .HasColumnName("MaterialID");
            entity.Property(e => e.Quantity).HasColumnType("int(11)");

            entity.HasOne(d => d.Location).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("inventory_ibfk_2");

            entity.HasOne(d => d.Material).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("inventory_ibfk_3");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PRIMARY");

            entity.ToTable("location");

            entity.Property(e => e.LocationId)
                .HasColumnType("int(11)")
                .HasColumnName("LocationID");
            entity.Property(e => e.LocationCapacity).HasColumnType("int(11)");
            entity.Property(e => e.LocationDescription).HasMaxLength(100);
            entity.Property(e => e.LocationName).HasMaxLength(50);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PRIMARY");

            entity.ToTable("material");

            entity.Property(e => e.MaterialId)
                .HasColumnType("int(11)")
                .HasColumnName("MaterialID");
            entity.Property(e => e.MaterialDescription)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.MaterialNumber).HasColumnType("int(11)");
            entity.Property(e => e.PriceUnit)
                .HasPrecision(10)
                .HasColumnName("price/unit");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .IsFixedLength();
        });

    

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("transaction");

            entity.HasIndex(e => e.MaterialId, "MaterialNumberID");

            entity.HasIndex(e => e.TransactionFromId, "TransactionFrom");

            entity.HasIndex(e => e.TransactionFromId, "TransactionFromID");

            entity.HasIndex(e => e.TransactionToId, "TransactionTo");

            entity.HasIndex(e => e.TransactionToId, "TransactionToID");

      
            entity.Property(e => e.TransactionId)
                .HasColumnType("int(11)")
                .HasColumnName("TransactionID");
            entity.Property(e => e.MaterialId)
                .HasColumnType("int(11)")
                .HasColumnName("MaterialID");
            entity.Property(e => e.TransactedQty).HasColumnType("int(11)");
            entity.Property(e => e.TransactionDateTime)
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionFromId)
                .HasColumnType("int(11)")
                .HasColumnName("TransactionFromID");
            entity.Property(e => e.TransactionToId)
                .HasColumnType("int(11)")
                .HasColumnName("TransactionToID");
          

            entity.HasOne(d => d.Material).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("transaction_ibfk_6");

            entity.HasOne(d => d.TransactionFrom).WithMany(p => p.TransactionTransactionFroms)
                .HasForeignKey(d => d.TransactionFromId)
                .HasConstraintName("transaction_ibfk_4");

            entity.HasOne(d => d.TransactionTo).WithMany(p => p.TransactionTransactionTos)
                .HasForeignKey(d => d.TransactionToId)
                .HasConstraintName("transaction_ibfk_5");

           
        });

     

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
