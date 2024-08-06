using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gestion.API.Users.Domain.Services;
using Gestion.API.Users.Interfaces.REST.Resources;
using Gestion.API.Users.Domain.Model.Commands;
using System.Threading.Tasks;
using Gestion.API.Users.Domain.Repositories;

namespace Gestion.API.Users.Interfaces.REST
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IUserRepository _userRepository;

        public AdminController(IUserCommandService userCommandService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userCommandService = userCommandService;
        }

       [Authorize(Roles = "Admin")]
        [HttpPost("admin/create")]
        public async Task<IActionResult> CreateAdminUser([FromBody] UserResource userResource)
        {
            var createUserCommand = new CreateUsersCommand
            {
                Name = userResource.Name,
                Email = userResource.Email,
                Password = userResource.Password,
                Role = userResource.Role  // Asignar el rol directamente
            };

            await _userCommandService.CreateUser(createUserCommand);

            return CreatedAtAction(nameof(CreateAdminUser), new { email = createUserCommand.Email }, createUserCommand);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("admin/update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserResource userResource)
        {
            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Name = userResource.Name;
            user.Email = userResource.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userResource.Password);
            user.Role = userResource.Role;

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("admin/delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            await _userRepository.DeleteAsync(user);

            return NoContent();
        }
    }
}
