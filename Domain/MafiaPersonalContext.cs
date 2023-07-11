using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public partial class MafiaPersonalContext : DbContext
{
    public MafiaPersonalContext()
    {
    }

    public MafiaPersonalContext(DbContextOptions<MafiaPersonalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AmmunitionType> AmmunitionTypes { get; set; }

    public virtual DbSet<FamilyMember> FamilyMembers { get; set; }

    public virtual DbSet<MafiaFamily> MafiaFamilies { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }

    public virtual DbSet<RankType> RankTypes { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Weapon> Weapons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost; Port=5432; DataBase=MafiaPersonal; Integrated Security=false; User ID=postgres; Password=super");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AmmunitionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AmmunitionTypes_pkey");

            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FamilyMembers_pkey");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.MafiaFamilyId).HasColumnName("MafiaFamily_ID");
            entity.Property(e => e.RankId).HasColumnName("Rank_ID");
            entity.Property(e => e.SecondName).HasMaxLength(50);

            entity.HasOne(d => d.MafiaFamily).WithMany(p => p.FamilyMembers)
                .HasForeignKey(d => d.MafiaFamilyId)
                .HasConstraintName("FamilyMembers_MafiaFamily_ID_fkey");

            entity.HasOne(d => d.Rank).WithMany(p => p.FamilyMembers)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FamilyMembers_Rank_ID_fkey");
        });

        modelBuilder.Entity<MafiaFamily>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MafiaFamilies_pkey");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(300)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Money).HasDefaultValueSql("0");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Organizations_pkey");

            entity.Property(e => e.CollectorId).HasColumnName("Collector_Id");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(300)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OrganizationTypeId).HasColumnName("OrganizationType_Id");

            entity.HasOne(d => d.Collector).WithMany(p => p.Organizations)
                .HasForeignKey(d => d.CollectorId)
                .HasConstraintName("Organizations_Collector_Id_fkey");

            entity.HasOne(d => d.OrganizationType).WithMany(p => p.Organizations)
                .HasForeignKey(d => d.OrganizationTypeId)
                .HasConstraintName("Organizations_OrganizationType_Id_fkey");
        });

        modelBuilder.Entity<OrganizationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OrganizationTypes_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<RankType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RankTypes_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Shop_pkey");

            entity.ToTable("Shop");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductName).HasMaxLength(50);
        });

        modelBuilder.Entity<Weapon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Weapons_pkey");

            entity.Property(e => e.AmmunitionTypeId).HasColumnName("AmmunitionType_Id");
            entity.Property(e => e.FamilyMemberId).HasColumnName("FamilyMember_ID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.AmmunitionType).WithMany(p => p.Weapons)
                .HasForeignKey(d => d.AmmunitionTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Weapons_AmmunitionType_Id_fkey");

            entity.HasOne(d => d.FamilyMember).WithMany(p => p.Weapons)
                .HasForeignKey(d => d.FamilyMemberId)
                .HasConstraintName("Weapons_FamilyMember_ID_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
