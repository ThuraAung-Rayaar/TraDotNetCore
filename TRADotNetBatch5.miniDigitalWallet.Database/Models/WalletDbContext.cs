using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TRADotNetBatch5.miniDigitalWallet.Database.Models;

public partial class WalletDbContext : DbContext
{
    public WalletDbContext()
    {
    }

    public WalletDbContext(DbContextOptions<WalletDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DepositAndWithdraw> DepositAndWithdraws { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<WalletUser> WalletUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = .;Initial Catalog =DigitalWallet ;User ID =sa; Password = sasa@123;Trust Server Certificate  = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DepositAndWithdraw>(entity =>
        {
            entity.HasKey(e => e.DepositId).HasName("PK__DepositA__AB60DF71FBA5B040");

            entity.ToTable("DepositAndWithdraw");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType).HasMaxLength(50);

            entity.HasOne(d => d.MobileNumberNavigation).WithMany(p => p.DepositAndWithdraws)
                .HasPrincipalKey(p => p.MobileNumber)
                .HasForeignKey(d => d.MobileNumber)
                .HasConstraintName("FK_DepositAndWithdraw_MobileNumber");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__Transact__9549009116CA6F96");

            entity.ToTable("Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(50);
            entity.Property(e => e.ReceiverMobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SenderMobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ReceiverMobileNoNavigation).WithMany(p => p.TransactionReceiverMobileNoNavigations)
                .HasPrincipalKey(p => p.MobileNumber)
                .HasForeignKey(d => d.ReceiverMobileNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Receiver");

            entity.HasOne(d => d.SenderMobileNoNavigation).WithMany(p => p.TransactionSenderMobileNoNavigations)
                .HasPrincipalKey(p => p.MobileNumber)
                .HasForeignKey(d => d.SenderMobileNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Sender");
        });

        modelBuilder.Entity<WalletUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__WalletUs__1788CC4C5F7BD16E");

            entity.ToTable("WalletUser");

            entity.HasIndex(e => e.MobileNumber, "UQ__WalletUs__250375B1B0FEFE08").IsUnique();

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PinCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
