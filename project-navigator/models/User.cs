namespace project_navigator.models;

public class RegistrationDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string HashedPassword { get; set; }
}