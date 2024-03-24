using Microsoft.EntityFrameworkCore;
using project_navigator.models;
using project_navigator.services;

namespace project_navigator.db;

public interface IDbDataInitializer
{
    public Task Initialize();
}

public class DbDataInitializer : IDbDataInitializer
{
    private readonly AppContext _dbContext;
    private readonly ISignService _signService;

    public DbDataInitializer(AppContext dbContext, ISignService signService)
    {
        _dbContext = dbContext;
        _signService = signService;
    }

    public async Task Initialize()
    {
        if (!await _dbContext.Database.CanConnectAsync())
            throw new InvalidOperationException("Unable to connect to the database.");

        if (!_dbContext.AccessLevels.Any())
            await _dbContext.AccessLevels.AddRangeAsync(new AccessLevel
            {
                Name = "Администратор"
            }, new AccessLevel
            {
                Name = "Обычный"
            });
        await _dbContext.SaveChangesAsync();
        if (!_dbContext.Users.Any())
        {
            var adminAccess = await _dbContext.AccessLevels.FirstOrDefaultAsync(level => level.Name == "Администратор");
            ArgumentNullException.ThrowIfNull(adminAccess);
            await _signService.RegisterAsync(new RegistrationDto("admin", "admin", adminAccess));
        }

        await _dbContext.SaveChangesAsync();
    }
}