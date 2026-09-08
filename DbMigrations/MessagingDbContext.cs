using Microsoft.EntityFrameworkCore;

namespace DbMigrations;

public class MessagingDbContext : DbContext
{
    public MessagingDbContext(DbContextOptions<MessagingDbContext> options)
        : base(options)
    {
        
    }
}