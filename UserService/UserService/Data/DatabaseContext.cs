using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<Coorporate> Coorporates { get; set; }
    }
}
