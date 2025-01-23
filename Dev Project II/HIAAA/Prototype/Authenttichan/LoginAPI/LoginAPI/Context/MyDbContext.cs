using System;
using System.Collections.Generic;
using LoginAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=cssql.cegep-heritage.qc.ca;Database=Scenario1_JMa_Test;User id=JMAGNAN;Password=password;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.AppId).HasName("PK__Applicat__6F8A0A34E80AD26D");

            entity.HasIndex(e => e.AppName, "UQ__Applicat__A5AEA8363B6CFFDD").IsUnique();

            entity.Property(e => e.AppId).HasColumnName("app_id");
            entity.Property(e => e.AppName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("app_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Applicati__creat__2C3393D0");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__E5331AFA47B9B66E");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.CanAddMembers)
                .HasDefaultValue(false)
                .HasColumnName("can_add_members");
            entity.Property(e => e.CanCreateApp)
                .HasDefaultValue(false)
                .HasColumnName("can_create_app");
            entity.Property(e => e.CanCreateRole)
                .HasDefaultValue(false)
                .HasColumnName("can_create_role");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Permissio__role___3B75D760");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC65161F56");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.AppId).HasColumnName("app_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");

            entity.HasOne(d => d.App).WithMany(p => p.Roles)
                .HasForeignKey(d => d.AppId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Roles__app_id__300424B4");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Roles__created_b__30F848ED");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F27C954FD");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164A84DA4DA").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC57209C4ADA3").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId, e.AppId }).HasName("PK__UserRole__5BB12B59C11E2013");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.AppId).HasColumnName("app_id");

            entity.HasOne(d => d.App).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.AppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__app_i__44FF419A");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__role___440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoles__user___4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
