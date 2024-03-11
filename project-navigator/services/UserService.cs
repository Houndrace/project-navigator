using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.models;

namespace project_navigator.services;

public interface IUserService
{
    public Task<bool> Authorize(string userName, string password, CancellationToken ct = default);
    public Task<bool> Register(RegistrationDto regDto, CancellationToken ct = default);
}

public class UserService : IUserService
{
    private readonly ProjNavContext _dbContext;
    private readonly IHashService _hashService;

    public UserService(IHashService hashService, ProjNavContext dbContext)
    {
        _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<bool> Authorize(string userName, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;


        try
        {
            var suggestedUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName, ct);
            return suggestedUser?.HashedPassword == _hashService.HashString(password);
        }
        catch (ArgumentNullException e)
        {
            throw new ArgumentNullException("Предоставленные данные не корректны.", e);
        }
        catch (OperationCanceledException e)
        {
            throw new OperationCanceledException("Операция была отменена.", e);
        }
        catch (Exception e)
        {
            throw new Exception("Произошла ошибка при попытке получить пользователя.", e);
        }
    }

    public async Task<bool> Register(RegistrationDto regDto, CancellationToken ct = default)
    {
        var hashedPassword = _hashService.HashString(regDto.Password);
        var user = new User
        {
            UserName = regDto.UserName,
            HashedPassword = hashedPassword
        };

        _dbContext.Users.Add(user);


        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateException e)
        {
            if (e is DbUpdateConcurrencyException concurrencyEx)
                Console.WriteLine($"Произошла ошибка конкуренции: {concurrencyEx.Message}");
            throw new DbUpdateException("Произошла ошибка при сохранении в базу данных.", e);
        }
        catch (OperationCanceledException e)
        {
            throw new OperationCanceledException("Операция была отменена.", e);
        }

        return true;
    }
}