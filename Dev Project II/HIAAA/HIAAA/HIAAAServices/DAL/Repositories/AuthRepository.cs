using System.Text;
using HIAAAServices.DAL.Interfaces;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HIAAAServices.DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly Hia3Context _context;
    private readonly IConfiguration _configuration;
    private readonly IRole _roleRepository;
    private readonly IUserRepository _userRepository;

    public AuthRepository(Hia3Context context, IConfiguration configuration, IRole roleRepository, IUserRepository userRepository)
    {
        _context = context;
        _configuration = configuration;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    public async Task<int> GetAppTypeByCode(string appCode)
    {
        var app = await _context.Apps.FirstOrDefaultAsync(a => a.Appcode == appCode);
        return app?.Apptype ?? 0;
    }

    public async Task<bool> AuthenticateLocally(string username, string passord)
    {
        var user = await GetUser(username);
        var localUser = await _context.LocalUsers.FirstOrDefaultAsync(u => u.Localuserid == user.Userid);
        return (user != null && localUser.Password == passord); // TODO: password hashing
    }
    
    public async Task<bool> AuthenticateHeritage(string username, string passord)
    {
        // TODO: investigate Entra ID to implement this
        return false; 
    }
    
    public async Task<bool> AuthenticateExternally(string username, string passord)
    {
        // TODO: implement in K50
        return false;
    }
    
    public async Task<User> GetUser(string username) {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user ?? null;
    }


    public async Task<string> GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Userid.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User> ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);
            
            return user;
        }
        catch
        {
            return null;
        }
    }
    
    public async Task<List<string>> GetUserRoles(string username, string appCode)
    {
        var roles = await _roleRepository.GetUserAppRoles(username, appCode);
        var roleCodes = new List<string>();
        foreach (var role in roles)
        {
            roleCodes.Add(role.Rolecode);
        }
        return roleCodes;
    }
    
}