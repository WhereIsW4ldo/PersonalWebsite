using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Database;

public class UserContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<LoginCredential> LoginCredentials { get; set; }

    public UserContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' not found.");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoginCredential>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Password).IsRequired();
        });
    }
}