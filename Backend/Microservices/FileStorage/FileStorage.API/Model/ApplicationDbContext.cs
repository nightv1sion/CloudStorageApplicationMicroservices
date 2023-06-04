using Microsoft.EntityFrameworkCore;

namespace FileStorage.API.Model;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }

    public DbSet<File> Files { get; set; }
}