using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Entities
{
    public class ReactionDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public ReactionDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<ReactionType> ReactionTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ReactionServiceDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reaction>()
                .HasData(
                new
                {
                    ReactionId = Guid.Parse("3b6d0a06-e64b-4f42-8689-fc10e8e6edf7"),
                    ReactionTypeId = 1,
                    PostId = Guid.Parse("15908a81-dcae-43e7-fecb-08d94eb2a3fe"),
                    AccountId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6")

                },
                new
                {
                    ReactionId = Guid.Parse("d8fe100b-8d5f-4027-961a-fa75bf8a3b94"),
                    ReactionTypeId = 2,
                    PostId = Guid.Parse("8ccb1467-9f38-4164-88da-15882fe82e58"),
                    AccountId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a")

                },
                new
                {
                    ReactionId = Guid.Parse("19e0acbf-5707-49ee-8cb6-134c00b7c10b"),
                    ReactionTypeId = 3,
                    PostId = Guid.Parse("23d2cce9-86d7-4bff-887e-f7712b16766d"),
                    AccountId = Guid.Parse("F2F88BCD-D0A2-4FE7-A23F-DF97A59731CD")
                });

            modelBuilder.Entity<ReactionType>()
               .HasData(
               new
               {
                   ReactionTypeId = 1,
                   TypeName = "Like"
               },
               new
               {
                   ReactionTypeId = 2,
                   TypeName = "Heart"
               },
               new
               {
                   ReactionTypeId = 3,
                   TypeName = "Smiley"
               });
        }
    }
}
