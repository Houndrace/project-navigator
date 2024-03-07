using project_navigator.db;
using project_navigator.models;

namespace project_navigator.services;

public interface IUserService
{
    public bool IsAuthorized(string userName, string password);
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

    public bool IsAuthorized(string userName, string password)
    {
        var suggestedUser = _dbContext.Users.FirstOrDefault(user => user.UserName == userName);

        if (suggestedUser == null)
            throw new InvalidOperationException("Пользователь с указанным именем не найден.");

        var providedHashPassword = _hashService.HashString(password);
        return suggestedUser.HashedPassword == providedHashPassword;
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