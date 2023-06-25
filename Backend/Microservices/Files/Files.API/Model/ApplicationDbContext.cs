using Microsoft.EntityFrameworkCore;

namespace Files.API.Model;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(
        DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Directory> Directories { get; set; }
}