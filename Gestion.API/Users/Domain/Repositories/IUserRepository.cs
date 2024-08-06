using Gestion.API.Users.Domain.Model.Aggregates;

namespace Gestion.API.Users.Domain.Repositories;

public interface IUserRepository
{
    Task<users> FindByEmailAsync(string email);
    Task AddAsync(users user);
    Task<int> GetUserDetailsAsync(int id);

    Task<int> GetUserDetailsAsyncEmail(string email);
    Task<users> FindByIdAsync(int id);

    Task<IEnumerable<users>> GetAllUsersAsync();
    Task UpdateAsync(users user);
    Task DeleteAsync(users user);
}