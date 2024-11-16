using Microsoft.EntityFrameworkCore;

namespace KPayEfcore.Database.Models
{
    public partial class AppDbContext : DbContext
    {
        
    public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<User_Tbl> Users { get; set; }
        public virtual DbSet<Pin_tbl> Pin_Codes { get; set; }
        public virtual DbSet<OTP_Tbl> OTP_Codes { get; set; }
        public virtual DbSet<First_Login_Tbl> FirstTimeLogins { get; set; }
        public virtual DbSet<Transaction_Tbl> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {

                string connectionString = "Data Source = .;Initial Catalog = KPayDotNetBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
                object value = optionsBuilder.UseSqlServer(connectionString);


            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User_Tbl>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Full_Name)
                .HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Phone_Number).HasMaxLength(15)
                .IsFixedLength();
            });

            modelBuilder.Entity<Pin_tbl>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.PinCode).HasMaxLength(6)
                .HasDefaultValue("");
            });

            modelBuilder.Entity<OTP_Tbl>(entity =>
            {
                entity.HasKey(e => e.Otp_Id);

                entity.Property(e => e.Otp_Code)
                .HasMaxLength(6).IsRequired();

                entity.Property(e => e.Type).
                HasMaxLength(20).IsRequired();

                entity.Property(e => e.Expire_Date)
                .HasColumnType("datetime");

            });

            modelBuilder.Entity<First_Login_Tbl>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Login_code)
                .HasMaxLength(6).IsRequired();

                entity.Property(e => e.First_Login_Date)
                .HasColumnType("datetime");

                entity.Property(e => e.Is_LoggedIn)
                .HasDefaultValue(false);

            });

            modelBuilder.Entity<Transaction_Tbl>(entity =>
            {
                entity.HasKey(e => e.Transaction_Id);

                entity.Property(e => e.senderId).IsRequired();

                entity.Property(e => e.receiverId);

                entity.Property(e => e.amount)
                .HasColumnType("decimal(10, 2)").IsRequired();

                entity.Property(e => e.Transaction_Date)
                .HasColumnType("datetime");

                entity.Property(e => e.Transaction_Type)
                .HasMaxLength(20).IsRequired();

                entity.Property(e => e.Notes).HasColumnType("varchar(MAX)");


            });
            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
