// File: Controllers/AuthController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginAPI.Models;
using LoginAPI.DAL;

namespace LoginAPI.Controllers {
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _repo;

        public AuthController(IConfiguration configuration, IAuthRepository repo)
        {
            _configuration = configuration;
            _repo = repo;
        }

        // Code 1 = Local DB
        // Code 2 = Heritage College
        // Code 3 = External App
        
        // POST: api/authenticate/1
        [HttpPost("authenticate/{code}")]
        public IActionResult Authenticate([FromBody] LoginViewModel model, int code)
        {
            User user = null;
            switch (code)
            {
                case 1:
                    var authenticated = _repo.Authenticated(model.Username, model.Password);
    
                    if (!authenticated)
                        return Unauthorized("Invalid username or password.");

                    user = _repo.GetUser(model.Username);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    return BadRequest("Invalid code given - not one of the options (1-3)");
            }
            
            // Generate JWT token
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var token = _repo.GenerateJwtToken(user, key);
            return Ok(new { token });
        }

        // GET: api/authorize/dkfnmjmcxhgnzjdfhsvj
        [HttpPost("authorize/{token}")]
        public async Task<IActionResult> Authorize(string token)
        {
            if (token == null)
                return Unauthorized("No token provided.");

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

                var userRoles = await _repo.GetUserRoles(userId);
                return Ok(userRoles);
            }
            catch
            {
                return Unauthorized("Invalid token.");
            }
        }

    }
}
