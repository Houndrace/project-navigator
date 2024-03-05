using Microsoft.EntityFrameworkCore;
using project_navigator.models;

namespace project_navigator.db;

public class ProjNavContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // TODO:Спрятать строку в конфиг файл и добавить в гитигнор
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=ProjectNavigator;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}