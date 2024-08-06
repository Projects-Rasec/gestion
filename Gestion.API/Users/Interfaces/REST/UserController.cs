using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gestion.API.Users.Domain.Services;
using Gestion.API.Users.Interfaces.REST.Resources;
using Gestion.API.Users.Domain.Model.Commands;
using System.Threading.Tasks;
using Gestion.API.Users.Domain.Repositories;

namespace Gestion.API.Users.Interfaces.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserCommandService _userCommandService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;

        public UsersController(IUserCommandService userCommandService, IAuthenticationService authenticationService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userCommandService = userCommandService;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserResource userResource)
        {
            var createUserCommand = new CreateUsersCommand
            {
                Name = userResource.Name,
                Email = userResource.Email,
                Password = userResource.Password,
                Role = userResource.Role  // Asignar el rol directamente
            };

            await _userCommandService.CreateUser(createUserCommand);

            return CreatedAtAction(nameof(CreateUser), new { email = createUserCommand.Email }, createUserCommand);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var token = await _authenticationService.Authenticate(login.Email, login.Password);

            if (token != null)
            {
                return Ok(new { token });
            }

            return Unauthorized();
        }

        

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            var userId = await _userRepository.GetUserDetailsAsync(id);

            if (userId == -1)
            {
                return NotFound("User not found.");
            }

            return Ok(new { UserId = userId });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
