using Microsoft.EntityFrameworkCore;

namespace Files.API.Model.Database;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(
        DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Directory>()
            .HasIndex(x => x.Id, "directories_id_uindex");
    }

    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Directory> Directories { get; set; }
}