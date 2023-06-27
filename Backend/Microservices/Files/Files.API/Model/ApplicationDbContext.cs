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
        modelBuilder.Entity<Directory>()
            .HasOne<Directory>(x => x.ParentDirectory)
            .WithMany(x => x.Directories)
            .HasForeignKey(x => x.ParentDirectoryId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }

    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Directory> Directories { get; set; }
}