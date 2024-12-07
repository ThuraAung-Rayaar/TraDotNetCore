using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DotNetBatch5.Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public AppDbContext() 
    {
    }
    public virtual DbSet<TblBlog> TblBlogs { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

    //    if (!optionsBuilder.IsConfigured) {

    //        string connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
    //        optionsBuilder.UseSqlServer(connectionString);


    //    }
    
    //}
/*#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=DotNetTraningBatch5;User Id=sa;Password=sasa@123;TrustServerCertificate=True;");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId);

            entity.ToTable("Tbl_Blog");

            entity.Property(e => e.BlogAuthor)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.BlogTitle).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
