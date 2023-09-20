using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence;

public class ApplicationDatabaseContext : IdentityDbContext<User>
{
    public ApplicationDatabaseContext(
        DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }
}