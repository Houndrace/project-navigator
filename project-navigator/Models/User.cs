namespace project_navigator.models;

public class RegistrationDto
{
    public RegistrationDto(string userName, string password, AccessLevel accessLevel)
    {
        UserName = userName;
        Password = password;
        AccessLevel = accessLevel;
    }

    public string UserName { get; }
    public string Password { get; }
    public AccessLevel AccessLevel { get; }
}

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string HashedPassword { get; set; }
    public virtual AccessLevel AccessLevel { get; set; }
}