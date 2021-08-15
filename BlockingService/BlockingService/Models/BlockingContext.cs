using BlockingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlockingService.Models
{
    public class BlockingContext :DbContext
    {
        public DbSet<Blocking> Blocks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Blocking;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Blocking>().HasKey(e => new { e.BlockerId, e.BlockedId });

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blocking>().HasData(new Blocking { BlockerId = Guid.Parse("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd"), BlockedId  = Guid.Parse("34a81ef8-2831-4444-8355-859d02ae2290") },
               new Blocking { BlockerId = Guid.Parse("34a81ef8-2831-4444-8355-859d02ae2290"), BlockedId = Guid.Parse("eb78bf24-a3ab-48bf-a931-7b0c6e680bfd") });
        }
    }
}
