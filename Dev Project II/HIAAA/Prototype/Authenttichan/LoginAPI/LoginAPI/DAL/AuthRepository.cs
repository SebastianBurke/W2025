using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using LoginAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginAPI.DAL {
    public class AuthRepository : IAuthRepository {
        private readonly Scenario1JmaTestContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(Scenario1JmaTestContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public bool Authenticated(string username, string password) {
            var user = GetUser(username);
            return (user != null && user.PasswordHash == password);
        }

        public User GetUser(string username) {
             return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public string GenerateJwtToken(User user, byte[] key) {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<List<UserRole>> GetUserRoles(int userId)
        {
            var userRoles = await _context.UserRoles.Include(ur => ur.Role).Where(ur => ur.UserId == userId).ToListAsync();
            return userRoles;
            // List<string> roles = new List<string>();
            // foreach (var role in userRoles)
            //     roles.Add(role.Role.RoleName);
            // return roles;
        }
    }
}
