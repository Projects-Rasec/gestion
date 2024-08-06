using BCrypt.Net;
using Gestion.API.Users.Domain.Model.Aggregates;
using Gestion.API.Users.Domain.Model.Commands;
using Gestion.API.Users.Domain.Repositories;
using Gestion.API.Users.Domain.Services;
using System;
using System.Threading.Tasks;
using Serilog;

namespace Gestion.API.Users.Application.CommandServices
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;

        public UserCommandService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUser(CreateUsersCommand command)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(command.Password);
                var user = new users(command.Name, command.Email, hashedPassword, command.Role);
                Log.Information("Creating user with role: {Role}", command.Role); // Agregar mensaje de depuración
                await _userRepository.AddAsync(user);
                Console.WriteLine($"User created: {user.Email} with Password Hash: {hashedPassword}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating user: {ex.Message}");
                throw;
            }
        }

    }
}