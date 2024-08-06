namespace Gestion.API.Users.Interfaces.REST.Transform;

using Gestion.API.Users.Domain.Model.Aggregates;
using Gestion.API.Users.Interfaces.REST.Resources;

public static class UserTransform
{
    public static UserResource ToResource(users user)
    {
        return new UserResource
        {
            Name = user.Name,
            Email = user.Email,
        };
    }

    public static users FromResource(UserResource resource)
    {
        return new users
        {
            Name = resource.Name,
            Email = resource.Email,
            Password = resource.Password // Asumiendo que users tiene una propiedad Password
        };
    }
}