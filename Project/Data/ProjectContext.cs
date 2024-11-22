using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext (DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }
        public DbSet<Project.Models.Product> Product { get; set; } 
        public DbSet<Project.Models.PCategory> PCategory { get; set; } 
        public DbSet<Project.Models.Cart> Cart { get; set; }
        public DbSet<Project.Models.OrderItem> OrderItem { get; set; } 
        public DbSet<Project.Models.OrderTable> OrderTable { get; set; } 
     
        public DbSet<Project.Models.Customer> Customer { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(c => new {c.PId, c.UId});
        }
    }
}
