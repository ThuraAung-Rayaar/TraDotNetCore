using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using traDotNetCore.ConsoleApp.Models;

namespace traDotNetCore.ConsoleApp
{
    public class AppDbContent :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
                     

            if (!optionsBuilder.IsConfigured) {
                string connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sa@123sa@123;TrustServerCertificate  = True";
                optionsBuilder.UseSqlServer(connectionString);
            
            }

            //

        }

        

        public DbSet<BlogDataModel> Blogs { get; set; }


    }
}
