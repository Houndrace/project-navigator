using Microsoft.EntityFrameworkCore;
using project_navigator.models;
using project_navigator.services;
using Serilog;

namespace project_navigator.db;

public class AppDbContext : DbContext
{
    private readonly IConfigurationService _configurationService;

    public AppDbContext(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public required DbSet<User> Users { get; set; }
    public required DbSet<AccessLevel> AccessLevels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = _configurationService.GetConnectionString();
        optionsBuilder.UseSqlServer(
            connectionString);
    }
}