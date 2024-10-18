using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListShell.ConsoleApp.Models
{
    public partial class AppDbContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured) {


                string connection = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
                optionsBuilder.UseSqlServer(connection);
            
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoDataModel>(entity =>
            {
              
                entity.HasKey(e => e.TaskID);

                
                entity.ToTable("ToDoList");

                
                entity.Property(e => e.TaskTitle)
                    .HasMaxLength(255)
                    .IsRequired(); // Since TaskTitle cannot be NULL

                
                entity.Property(e => e.TaskDescription)
                    .HasColumnType("text") // TaskDescription is of type 'text'
                    .IsRequired(false); // Nullable column

                
                entity.Property(e => e.CategoryID)
                    .IsRequired(false); // Nullable column

               
                entity.Property(e => e.PriorityLevel)
                    .HasColumnType("tinyint")
                    .IsRequired(); // PriorityLevel cannot be NULL

                
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(false); // Nullable column

                
                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .IsRequired(); // Nullable column

                
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .IsRequired(); // CreatedDate cannot be NULL

                entity.Property(e=>e.DeleteFlag).HasColumnType("boolean");
                entity.Property(e => e.CompletedDate)
                    .HasColumnType("datetime")
                    .IsRequired(false); // Nullable column
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public DbSet<ToDoDataModel> ToDoLists { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
