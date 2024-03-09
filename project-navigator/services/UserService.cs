using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.models;

namespace project_navigator.services;

public interface IUserService
{
    public Task<bool> IsAuthorized(string userName, string password);
    public void Register(RegistrationDto regDto);
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

    public async Task<bool> IsAuthorized(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;

        var suggestedUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        return suggestedUser?.HashedPassword == _hashService.HashString(password);
    }

    public void Register(RegistrationDto regDto)
    {
        var hashedPassword = _hashService.HashString(regDto.Password);
        var user = new User
        {
            UserName = regDto.UserName,
            HashedPassword = hashedPassword
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
}