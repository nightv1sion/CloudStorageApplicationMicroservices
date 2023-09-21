using Microsoft.EntityFrameworkCore;
using Directory = Files.Domain.Entities.Directory.Directory;
using File = Files.Domain.Entities.File.File;
namespace Files.Infrastructure.Persistence;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Directory> Directories { get; set; }
}