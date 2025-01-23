using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices.Models;

public partial class Hia3Context : DbContext
{
    public Hia3Context()
    {
    }

    public Hia3Context(DbContextOptions<Hia3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<App> Apps { get; set; }

    public virtual DbSet<AppUserRole> AppUserRoles { get; set; }

    public virtual DbSet<LocalUser> LocalUsers { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=cssql.cegep-heritage.qc.ca;Database=HIA3_PB;User id=PBADRA;Password=password;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<App>(entity =>
        {
            entity.HasKey(e => e.Appid).HasName("PK__Apps__C00F024DBDD75F6C");

            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.Appcode)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("appcode");
            entity.Property(e => e.Appname)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("appname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Apps)
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Apps_createdby_fk");
        });

        modelBuilder.Entity<AppUserRole>(entity =>
        {
            entity.HasKey(e => e.Appuserroleid).HasName("PK__AppUserR__4E5E604733041C16");

            entity.Property(e => e.Appuserroleid).HasColumnName("appuserroleid");
            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.App).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.Appid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Apps_appid_fk");

            entity.HasOne(d => d.Role).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Roles_roleid_fk");

            entity.HasOne(d => d.User).WithMany(p => p.AppUserRoles)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_userid_fk");
        });

        modelBuilder.Entity<LocalUser>(entity =>
        {
            entity.HasKey(e => e.Localuserid).HasName("PK__LocalUse__9ACA3501E89F75A9");

            entity.Property(e => e.Localuserid)
                .ValueGeneratedOnAdd()
                .HasColumnName("localuserid");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("password");

            entity.HasOne(d => d.Localuser).WithOne(p => p.LocalUser)
                .HasForeignKey<LocalUser>(d => d.Localuserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LocalUsers_localuserid_fk");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Logid).HasName("PK__Logs__7838F265F9475F8E");

            entity.Property(e => e.Logid).HasColumnName("logid");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Logevent)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("logevent");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Logs_userid_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("PK__Roles__CD994BF2C3ED149E");

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolecode)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("rolecode");
            entity.Property(e => e.Rolename)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__Users__CBA1B257F144F80C");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Username)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
