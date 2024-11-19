using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRADotNetCore.POS.Database.Models;

public partial class AppDbContext :DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){ }

    public virtual DbSet<ProductDetail> Products { get; set; }
    public virtual DbSet<ProductInStock> InStocks { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<ShopDetail> Shops { get; set; } 
    public virtual DbSet<StaffDetail> Staffs { get; set; } 
    public virtual DbSet<SaleInvoice> SaleInvoices { get; set; } 
    public virtual DbSet<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {

            string connectionString = "Data Source = .;Initial Catalog = POSDotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
            object value = optionsBuilder.UseSqlServer(connectionString);


        }

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.ProductCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProductCategoryCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProductName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)").IsRequired();
        });

        modelBuilder.Entity<ProductInStock>(entity =>
        {
           
            entity.Property(e => e.ProductCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.StockDate).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Count).HasColumnType("int").IsRequired();
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId);
            entity.Property(e => e.ProductCategoryCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProductCategoryName).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<ShopDetail>(entity =>
        {
            entity.HasKey(e => e.ShopId);
            entity.Property(e => e.ShopCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ShopName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.MobileNum).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<StaffDetail>(entity =>
        {
            entity.HasKey(e => e.StaffId);
            entity.Property(e => e.StaffCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.StaffName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.MobileNum).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Gender).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Password).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<SaleInvoice>(entity =>
        {
            entity.HasKey(e => e.SaleInvoiceId);
            entity.Property(e => e.SaleInvoiceDateTime).IsRequired();
            entity.Property(e => e.VoucherNo).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(e => e.StaffCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(e => e.PaymentType).HasMaxLength(10);
            entity.Property(e => e.CustomerAccountNum).HasMaxLength(20);
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReceiveAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChangeAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerCode).HasMaxLength(50);
        });

        modelBuilder.Entity<SaleInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.SaleInvoiceDetailId);
            entity.Property(e => e.VoucherNo).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ProductCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)").IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)").IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
