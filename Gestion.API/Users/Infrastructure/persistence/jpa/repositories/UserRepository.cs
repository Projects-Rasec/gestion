using System.Data;
using Gestion.API.Shared.Infrastructure;
using Gestion.API.Users.Domain.Model.Aggregates;
using Gestion.API.Users.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Gestion.API.Users.Infrastructure.persistence.jpa.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<users> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }  

        public async Task<int> GetUserDetailsAsyncEmail(string email) 
        {
            var userIdParam = new SqlParameter("@UserId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var emailParam = new SqlParameter("@Email", email);

            await _context.Database.ExecuteSqlRawAsync("EXEC GetUserDetails @Email, @UserId OUTPUT", emailParam, userIdParam);

            int userId = (int)userIdParam.Value;

            return userId;
        }

        public async Task<users> FindByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(users user)
        {
            var name = new SqlParameter("@Name", user.Name);
            var email = new SqlParameter("@Email", user.Email);
            var password = new SqlParameter("@Password", user.Password);
            var role = new SqlParameter("@Role", user.Role.ToString());

            await _context.Database.ExecuteSqlRawAsync("EXEC AddUser @Name, @Email, @Password, @Role", name, email, password, role);
        }

        public async Task<int> GetUserDetailsAsync(int id)
        {
            var userIdParam = new SqlParameter("@UserId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var idParam = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync("EXEC GetUserDetails @Id, @UserId OUTPUT", idParam, userIdParam);

            int userId = (int)userIdParam.Value;

            return userId;
        }

        public async Task<IEnumerable<users>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateAsync(users user)
        {
            var id = new SqlParameter("@Id", user.Id);
            var name = new SqlParameter("@Name", user.Name);
            var email = new SqlParameter("@Email", user.Email);
            var password = new SqlParameter("@Password", user.Password);
            var role = new SqlParameter("@Role", user.Role.ToString());

            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateUser @Id, @Name, @Email, @Password, @Role", id, name, email, password, role);
        }

        public async Task DeleteAsync(users user)
        {
            var id = new SqlParameter("@Id", user.Id);

            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteUser @Id", id);
        }
    }
}
