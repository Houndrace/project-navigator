using Microsoft.EntityFrameworkCore;
using project_navigator.models;
using project_navigator.services;

namespace project_navigator.db;

public interface IDbDataInitializer
{
    public Task InitializeAsync();
    public Task Initialize();
}

public class DbDataInitializer : IDbDataInitializer
{
    private readonly AppContext _dbContext;
    private readonly ISignService _signService;

    private readonly IEnumerable<AccessLevel> _accessLevels = new List<AccessLevel>
    {
        new()
        {
            Name = "Администратор"
        },
        new()
        {
            Name = "Обычный"
        }
    };

    public DbDataInitializer(AppContext dbContext, ISignService signService)
    {
        _dbContext = dbContext;
        _signService = signService;
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();
        if (!await _dbContext.AccessLevels.AnyAsync())
            await _dbContext.AccessLevels.AddRangeAsync(_accessLevels);

        if (!await _dbContext.Users.AnyAsync())
        {
            var adminAccess = _dbContext.AccessLevels.Local.FirstOrDefault(level => level.Name == "Администратор");
            ArgumentNullException.ThrowIfNull(adminAccess);
            await _signService.RegisterAsync(new RegistrationDto("admin", "admin", adminAccess));
        }

        await _dbContext.SaveChangesAsync();
    }

    public Task Initialize()
    {
        _dbContext.Database.Migrate();
        if (!_dbContext.AccessLevels.Any())
            _dbContext.AccessLevels.AddRange(_accessLevels);

        if (!_dbContext.Users.Any())
        {
            var adminAccess = _dbContext.AccessLevels.Local.FirstOrDefault(level => level.Name == "Администратор");
            ArgumentNullException.ThrowIfNull(adminAccess);
            _signService.RegisterAsync(new RegistrationDto("admin", "admin", adminAccess));
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
}