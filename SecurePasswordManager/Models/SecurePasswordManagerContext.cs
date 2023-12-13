using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SecurePasswordManager.Models;

public partial class SecurePasswordManagerContext : DbContext
{
    public SecurePasswordManagerContext()
    {
    }

    public SecurePasswordManagerContext(DbContextOptions<SecurePasswordManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PasswordRecord> PasswordRecords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=SecurePasswordManager;Integrated Security=True;Initial Catalog=SecurePasswordManager;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PasswordRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Password__FBDF78E953020584");
            entity.Property(e => e.RecordId).ValueGeneratedOnAdd();
            entity.Property(e => e.AdditionalInfo).HasMaxLength(500);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PlatformName).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.PasswordRecords)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordR__UserI__3A81B327");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnType("int");
            entity.HasKey(e => e.Username).HasName("PK__Users__1788CC4C09E2148A");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E46A9CC10C").IsUnique();
            
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Salt).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
