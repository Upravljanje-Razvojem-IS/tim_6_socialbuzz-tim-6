using Microsoft.EntityFrameworkCore;
using ReportingService.Entities;

namespace ReportingService.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Report> Chats { get; set; }
    }
}
