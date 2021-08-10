using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostService.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities
{
    public class PostDbContext : DbContext
    {

        private readonly IConfiguration configuration;

        public PostDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PostHistory> PostHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PostServiceDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new
                {
                    PostId = Guid.Parse("b43b4683-7d9d-413f-a6e1-f821a80c8ce4"),
                    PostName = "Adidas sportska majica",
                    PostImage = "https://www.intersport.rs/media/catalog/product/cache/382907d7f48ae2519bf16cd5f39b77f9/g/k/gk9635_app_photo_front-center_white.jpg",
                    Description = "Adidas Essentials T-Shirt je muška majica koja je pogodna za mnoge sportske i zabavne aktivnosti. Napravljena je od prijatne mešavine prirodnih i veštačkih materijala.",
                    Price = 2590.00,
                    Currency = "Rsd",
                    Category = "Sportska odeća",
                    PublicationDate = DateTime.Parse("2020-05-20T09:00:00"),
                    AccountId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a"),
                    Weight = "130g"
                },
                new
                {
                    PostId = Guid.Parse("8ccb1467-9f38-4164-88da-15882fe82e58"),
                    PostName = "Iphone 11",
                    PostImage = "https://www.tehnomedia.rs/image/71354.jpg?tip=huge&tip_slike=0",
                    Description = "Nov, neotpakovan Iphone 11 64GB. Garancija godinu dana.",
                    Price = 560.00,
                    Currency = "Euro",
                    Category = "Mobilni telefoni",
                    PublicationDate = DateTime.Parse("2021-06-11T11:00:00"),
                    AccountId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6"),
                    Weight = "194g"
                },
                new
                {
                    PostId = Guid.Parse("23d2cce9-86d7-4bff-887e-f7712b16766d"),
                    PostName = "Adidas Adizero Boston 9",
                    PostImage = "https://www.sportvision.rs/files/thumbs/files/images/slike_proizvoda/media/FY0/FY0343/images/thumbs_600/FY0343_600_600px.jpg",
                    Description = "Adidas adizero Boston 9 M su muške patike za trčanje koje su namenjene za trčanje po urbanim površinama. Stvorene su za vrhunske trkače. Đon je napravljen od Boost pene, a Continental guma pruža odlično prijanjanje.",
                    Price = 14690.00,
                    Currency = "Rsd",
                    Category = "Patike za trčanje",
                    PublicationDate = DateTime.Parse("2021-04-17T08:00:00"),
                    AccountId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6"),
                    Weight = "238g"
                });

            modelBuilder.Entity<Service>().HasData(
                new
                {
                    PostId = Guid.Parse("5284a73f-1f9e-4799-a793-5a4fe4a1df56"),
                    PostName = "Šminkanje",
                    PostImage = "",
                    Description = "Zablistajte uz najnoviju šminku vrhunskog kvaliteta.",
                    Price = 2500.00,
                    Currency = "Rsd",
                    Category = "Lepota i zdravlje",
                    PublicationDate = DateTime.Parse("2021-06-12T12:00:00"),
                    AccountId = Guid.Parse("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd")
                },
                new
                {
                    PostId = Guid.Parse("54f9baf6-271e-40cb-8d80-a27980fc8b63"),
                    PostName = "Muško šišanje",
                    PostImage = "",
                    Description = "Zakaži svoj termin i dobij frizuru baš po tvojoj želji",
                    Price = 450.00,
                    Currency = "Rsd",
                    Category = "Muški frizer",
                    PublicationDate = DateTime.Parse("2021-07-07T12:00:00"),
                    AccountId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a")
                });

            modelBuilder.Entity<PostHistory>().HasData(
                new
                {
                    PostHistoryId = 1,
                    Price = 2499.00,
                    DateFrom = DateTime.Parse("2020-05-20T09:00:00"),
                    DateTo = DateTime.Parse("2020-10-05T09:00:00"),
                    PostId = Guid.Parse("b43b4683-7d9d-413f-a6e1-f821a80c8ce4")
                },
                new
                {
                    PostHistoryId = 2,
                    Price = 2590.00,
                    DateFrom = DateTime.Parse("2020-10-05T09:00:00"),
                    PostId = Guid.Parse("b43b4683-7d9d-413f-a6e1-f821a80c8ce4")
                },
                new
                {
                    PostHistoryId = 3,
                    Price = 560.00,
                    DateFrom = DateTime.Parse("2021-06-11T11:00:00"),
                    PostId = Guid.Parse("8ccb1467-9f38-4164-88da-15882fe82e58")
                },
                new
                {
                    PostHistoryId = 4,
                    Price = 14690.00,
                    DateFrom = DateTime.Parse("2021-04-17T08:00:00"),
                    PostId = Guid.Parse("23d2cce9-86d7-4bff-887e-f7712b16766d")
                },
                new
                {
                    PostHistoryId = 5,
                    Price = 2500.00,
                    DateFrom = DateTime.Parse("2021-06-12T12:00:00"),
                    PostId = Guid.Parse("5284a73f-1f9e-4799-a793-5a4fe4a1df56")
                },
                new
                {
                    PostHistoryId = 6,
                    Price = 450.00,
                    DateFrom = DateTime.Parse("2021-07-07T12:00:00"),
                    PostId = Guid.Parse("54f9baf6-271e-40cb-8d80-a27980fc8b63")
                });
        }
    }
}
