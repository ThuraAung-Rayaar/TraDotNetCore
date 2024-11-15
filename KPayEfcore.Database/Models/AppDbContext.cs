using Microsoft.EntityFrameworkCore;

namespace KPayEfcore.Database.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {




        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Pincode> PinCodes { get; set; } = null!;
        public virtual DbSet<FirstTimeLogin> FirstTimeLogin { get; set; } = null!;
        public virtual DbSet<OTPCode> OtpCodes { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<Receipt> Receipts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Data Source=.;Initial Catalog=KPayDotNetBatch5;User ID=sa;Password=sasa@123;TrustServerCertificate=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15).IsRequired();
                entity.Property(e => e.UserName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedDate).HasDefaultValueSql("GETDATE()");
                //entity.Property(e => e.pin).HasDefaultValue(null);
            });
            modelBuilder.Entity<Pincode>(x =>

            {
                x.HasKey(e => e.UserId);
                x.Property(e => e.PinCode).HasMaxLength(6).HasDefaultValue(null);
            
            
            }
            );
            modelBuilder.Entity<FirstTimeLogin>(entity =>
            {
                entity.HasKey(e => e.FirstLoginId);
                entity.Property(e => e.FirstTimeCode).HasMaxLength(6).IsRequired();
                //entity.HasOne(d => d.User)
                //      .WithMany(p => p.FirstTimeLogins)
                //      .HasForeignKey(d => d.UserId)
                //      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OTPCode>(entity =>
            {
                entity.HasKey(e => e.OtpId);
                entity.Property(e => e.OtpCodeValue).HasMaxLength(6).IsRequired();
                entity.Property(e => e.OtpStatus).HasMaxLength(20).IsRequired();
                entity.Property(e=>e.OtpType).HasMaxLength(20).IsRequired();
                //entity.HasOne(d => d.User)
                //      .WithMany(p => p.OtpCodes)
                //      .HasForeignKey(d => d.UserId)
                //      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.Property(e => e.TransactionType).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.TransactionTime).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.BalanceAfter).HasColumnType("decimal(10, 2)").IsRequired();
                entity.Property(e => e.Note).HasColumnType("varchar(MAX)").IsRequired();
                //entity.HasOne(d => d.User)
                //      .WithMany(p => p.Transactions)
                //      .HasForeignKey(d => d.UserId)
                //      .OnDelete(DeleteBehavior.Cascade);

                //entity.HasOne(d => d.Receipt)
                //      .WithOne(p => p.Transaction)
                //      .HasForeignKey<Transaction>(d => d.ReceiptId);
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.ReceiptId);
                entity.Property(e => e.ReceiptContent).IsRequired();
                entity.Property(e => e.IssuedDate).HasDefaultValueSql("GETDATE()");
                //entity.HasOne(d => d.Transaction)
                //      .WithOne(p => p.Receipt)
                //      .HasForeignKey<Receipt>(d => d.TransactionId)
                //      .OnDelete(DeleteBehavior.Cascade);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
