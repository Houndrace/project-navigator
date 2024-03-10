using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.models;

namespace project_navigator.services;

public interface IUserService
{
    public Task<bool> Authorize(string userName, string password);
    public Task<bool> Register(RegistrationDto regDto);
}

public class UserService : IUserService
{
    private readonly ProjNavContext _dbContext;
    private readonly IHashService _hashService;

    public UserService(IHashService hashService, ProjNavContext dbContext)
    {
        _hashService = hashService;
        _dbContext = dbContext;
    }

    public async Task<bool> Authorize(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;

        User? suggestedUser;
        try
        {
            suggestedUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
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

        return suggestedUser?.HashedPassword == _hashService.HashString(password);
    }

    public async Task<bool> Register(RegistrationDto regDto)
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
            var f = await _dbContext.SaveChangesAsync();
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