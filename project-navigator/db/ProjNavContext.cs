using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_navigator.infrastructure;
using project_navigator.models;

namespace project_navigator.db;

public class ProjNavContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //TODO:разобраться со строкой в продакшене
        var configuration = AppConfigurations.GetEnvConfig();
        var conString = configuration.GetConnectionString("Db");

        optionsBuilder.UseSqlServer(conString);
    }
}