namespace Gestion.API.Users.Domain.Model.Queries;

public class GetUserByEmailAndPasswordQuery
{
    public string Email { get; set; }
    public string Password { get; set; }
}