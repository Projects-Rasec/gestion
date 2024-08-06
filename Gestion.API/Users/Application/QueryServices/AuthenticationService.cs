using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gestion.API.Users.Domain.Model.Aggregates;
using Gestion.API.Users.Domain.Repositories;
using Gestion.API.Users.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Serilog;

namespace Gestion.API.Users.Application.QueryServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(string email, string password)
        {
            Log.Information("Authenticating user: {Email}", email);

            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                Log.Warning("User not found: {Email}", email);
                return null;
            }

            Log.Information("Stored password hash for user {Email}: {PasswordHash}", email, user.Password);
            Log.Information("Entered password for user {Email}: {Password}", email, password);
            Log.Information("Stored password hash length: {Length}", user.Password.Length);
            
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            Log.Information("Password verification result for user {Email}: {Result}", email, isPasswordValid);

            if (!isPasswordValid)
            {
                Log.Warning("Invalid password for user: {Email}", email);
                return null;
            }

            Log.Information("User authenticated successfully: {Email}", email);
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(users user)
        {
            string jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key configuration is missing.");
            string jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer configuration is missing.");
            string jwtAudience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience configuration is missing.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()) ,
                new Claim("email", user.Email),
                new Claim("username", user.Name) 
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
