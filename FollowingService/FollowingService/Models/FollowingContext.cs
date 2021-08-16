using FollowingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace FollowingService.Model
{
    public class FollowingContext : DbContext
    {
        public DbSet<Following> Follows { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Following;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Following>().HasKey(e => new { e.FollowerId, e.FollowingId });

            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Following>().HasData(new Following { FollowerId = Guid.Parse("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd"), FollowingId = Guid.Parse("34a81ef8-2831-4444-8355-859d02ae2290") },
              new Following { FollowerId = Guid.Parse("34a81ef8-2831-4444-8355-859d02ae2290"), FollowingId = Guid.Parse("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd") });
        }
    }
}
