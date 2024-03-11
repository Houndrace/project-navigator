using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.models;

namespace project_navigator.services;

/// <summary>
/// Interface defining the user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously authorizes a user based on username and password.
    /// </summary>
    /// <param name="userName">The user's username.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="ct">Cancellation token for async operations.</param>
    /// <returns>A task representing the asynchronous operation, containing true if authorization succeeds, otherwise false.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    public Task<bool> AuthorizeAsync(string userName, string password, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously registers a new user using the provided registration data.
    /// </summary>
    /// <param name="regDto">A data transfer object containing the user's registration information.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether registration was successful.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided registration data transfer object is null.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Thrown when a database concurrency error occurs during registration.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled through the cancellation token.</exception>
    public Task<bool> RegisterAsync(RegistrationDto regDto, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves a user based on their username from the database.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found, otherwise null.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled through the cancellation token.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the provided username is null.</exception>
    public Task<User?> GetUserAsync(string userName, CancellationToken ct = default);
}

/// <summary>
/// Implements the operations defined in the IUserService interface, providing services for user authentication and registration.
/// </summary>
public class UserService : IUserService
{
    private readonly ProjNavContext _dbContext;
    private readonly IHashService _hashService;

    /// <summary>
    /// Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="hashService">The service used for hashing passwords.</param>
    /// <param name="dbContext">The database context used for data access.</param>
    public UserService(IHashService hashService, ProjNavContext dbContext)
    {
        _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task<bool> AuthorizeAsync(string userName, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;

        var suggestedUser = await GetUserAsync(userName, ct);
        return suggestedUser?.HashedPassword == _hashService.HashString(password);
    }

    /// <inheritdoc />
    public async Task<bool> RegisterAsync(RegistrationDto regDto, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(regDto);

        var hashedPassword = _hashService.HashString(regDto.Password);
        var user = new User
        {
            UserName = regDto.UserName,
            HashedPassword = hashedPassword
        };

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    /// <inheritdoc />
    public async Task<User?> GetUserAsync(string userName, CancellationToken ct = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName, ct);
    }
}