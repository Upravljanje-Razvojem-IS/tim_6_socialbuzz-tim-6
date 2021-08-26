using Microsoft.EntityFrameworkCore;
using SasaMessagingService.Entities;

namespace SasaMessagingService.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageStatus> MessageStatuses { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
    }
}
