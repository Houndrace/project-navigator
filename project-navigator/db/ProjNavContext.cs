using Microsoft.EntityFrameworkCore;
using project_navigator.models;
using project_navigator.services;

namespace project_navigator.db;

public class ProjNavContext : DbContext
{
    private readonly IConfigurationService _configurationService;

    public ProjNavContext(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var connectionString = _configurationService.GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString);
    }
}