using Microsoft.EntityFrameworkCore.Design;
using project_navigator.services;

namespace project_navigator.db;

public class DesignAppContext : IDesignTimeDbContextFactory<AppContext>
{
    public AppContext CreateDbContext(string[] args)
    {
        return new AppContext(new ConfigurationService());
    }
}