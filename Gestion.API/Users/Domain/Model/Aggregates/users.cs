namespace Gestion.API.Users.Domain.Model.Aggregates;

public class users
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }  // Añadir esta línea para el rol

    public users(string name, string email, string password, UserRole role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public users() { }
}