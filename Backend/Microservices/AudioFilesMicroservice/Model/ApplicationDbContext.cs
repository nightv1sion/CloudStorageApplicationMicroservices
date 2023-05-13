using Microsoft.EntityFrameworkCore;

namespace AudioFilesMicroservice.Model;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions options) : base(options)
    {
        
    }
}