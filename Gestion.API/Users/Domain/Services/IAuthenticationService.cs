namespace Gestion.API.Users.Domain.Services
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(string email, string password); // Autentica al usuario y devuelve un token JWT
    }
}