using Microsoft.EntityFrameworkCore;
using project_navigator.db;
using project_navigator.models;

namespace project_navigator.services;

/// <summary>
///     Interface defining the user service operations.
/// </summary>
public interface ISignService
{
    User? LastSignedInUser { get; }

    /// <summary>
    ///     Asynchronously authorizes a user based on username and password.
    /// </summary>
    /// <param name="userName">The user's username.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="ct">Cancellation token for async operations.</param>
    /// <returns>A task representing the asynchronous operation, containing true if authorization succeeds, otherwise false.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    Task<bool> SignInAsync(string userName, string password, CancellationToken ct = default);

    /// <summary>
    ///     Asynchronously registers a new user using the provided registration data.
    /// </summary>
    /// <param name="regDto">A data transfer object containing the user's registration information.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a boolean indicating whether
    ///     registration was successful.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided registration data transfer object is null.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Thrown when a database concurrency error occurs during registration.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled through the cancellation token.</exception>
    Task RegisterAsync(RegistrationDto regDto, CancellationToken ct = default);
}

/// <summary>
///     Implements the operations defined in the IUserService interface, providing services for user authentication and
///     registration.
/// </summary>
public class SignService : ISignService
{
    private readonly AppDbContext _dbDbContext;
    private readonly IHashService _hashService;

    /// <summary>
    ///     Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="hashService">The service used for hashing passwords.</param>
    /// <param name="dbDbContext">The database context used for data access.</param>
    public SignService(IHashService hashService, AppDbContext dbDbContext)
    {
        _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
        _dbDbContext = dbDbContext ?? throw new ArgumentNullException(nameof(dbDbContext));
    }

    public User? LastSignedInUser { get; private set; }

    /// <inheritdoc />
    public async Task<bool> SignInAsync(string userName, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            return false;

        var suggestedUser = await GetUserAsync(userName, ct);

        if (suggestedUser?.HashedPassword != _hashService.HashString(password))
            return false;

        LastSignedInUser = suggestedUser;
        return true;
    }

    /// <inheritdoc />
    public async Task RegisterAsync(RegistrationDto regDto, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(regDto);

        var hashedPassword = _hashService.HashString(regDto.Password);
        var user = new User
        {
            UserName = regDto.UserName,
            HashedPassword = hashedPassword,
            AccessLevel = regDto.AccessLevel
        };

        var existentUser = await GetUserAsync(regDto.UserName, ct);

        if (existentUser != null)
            throw new InvalidOperationException($"A user with the username '{regDto.UserName}' already exists.");

        _dbDbContext.Users.Add(user);

        await _dbDbContext.SaveChangesAsync(ct);
    }

    private async Task<User?> GetUserAsync(string userName, CancellationToken ct = default)
    {
        return await _dbDbContext.Users.Where(user => user.UserName == userName)
            .Include(user => user.AccessLevel)
            .FirstOrDefaultAsync(ct);
    }
}