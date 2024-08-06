using Gestion.API.Users.Domain.Model.Commands;

namespace Gestion.API.Users.Domain.Services;

public interface IUserCommandService
{
    Task CreateUser(CreateUsersCommand command);
}