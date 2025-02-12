using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Database;

public class UserContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<LoginCredential> LoginCredentials { get; set; }

    public UserContext(IConfiguration configuration, ILogger logger)
    {
        var dbConnectionSection = configuration.GetSection("DatabaseConnectionStrings");
        
        var connectionString = dbConnectionSection["Database"]
                               ?? throw new InvalidOperationException("Connection string 'Database' not found.");

        var environmentPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var password = string.IsNullOrEmpty(environmentPassword)
          ? dbConnectionSection["DefaultPassword"]
          : environmentPassword;

        _connectionString = string.Format(connectionString, password);
        logger.Information("connectionString: {connectionString}", _connectionString);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(_connectionString);

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
