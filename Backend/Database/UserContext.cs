using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class UserContext : DbContext
{
    public DbSet<LoginCredential> LoginCredentials { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}