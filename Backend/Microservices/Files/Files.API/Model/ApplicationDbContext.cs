﻿using Microsoft.EntityFrameworkCore;

namespace Files.API.Model;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }

    public virtual DbSet<File> Files { get; set; }
}