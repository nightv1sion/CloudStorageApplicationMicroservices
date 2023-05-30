using Microsoft.EntityFrameworkCore;

namespace Audio.API.Model;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<AudioFile> AudioFiles { get; set; }
}