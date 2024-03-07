using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_navigator.models;

namespace project_navigator.db;

public class ProjNavContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ProjNavContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //TODO:разобраться со строкой в продакшене
        var conString = _configuration.GetConnectionString("Db");
        optionsBuilder.UseSqlServer(conString);
    }
}