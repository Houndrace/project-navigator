using Microsoft.EntityFrameworkCore;
using project_navigator.models;
using project_navigator.services;
using Serilog;

namespace project_navigator.db;

public class AppContext : DbContext
{
    private readonly IConfigurationService _configurationService;

    public AppContext(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<AccessLevel> AccessLevels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = _configurationService.GetConnectionString();
        optionsBuilder.UseSqlServer(
            connectionString);
    }
}