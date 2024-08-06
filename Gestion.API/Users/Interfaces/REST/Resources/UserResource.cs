using Gestion.API.Users.Domain.Model.Aggregates;

namespace Gestion.API.Users.Interfaces.REST.Resources;

public class UserResource
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public UserRole Role { get; set; }  // Añadir esta línea para el rol

}
