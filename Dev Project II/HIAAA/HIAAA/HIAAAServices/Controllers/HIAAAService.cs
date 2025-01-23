using System.Text;
using HIAAAServices.DAL.Interfaces;
using HIAAAServices.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HIAAAServices.Controllers;

[ApiController]
[Route("HIA3/REST")]
public class HIAAAService : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public HIAAAService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    
    [HttpPost("Authenticate")]
    public async Task<IActionResult> Authenticate([FromQuery] string username, [FromQuery] string password, [FromQuery] string appCode)
    {
        var appType = await _authRepository.GetAppTypeByCode(appCode);
        var authenticated = false;
        
        switch (appType)
        {
            case 1: // Local
                authenticated = await _authRepository.AuthenticateLocally(username, password);
                break;
            case 2: // Heritage
                authenticated = await _authRepository.AuthenticateHeritage(username, password);
                break;
            case 3: // External
                authenticated = await _authRepository.AuthenticateExternally(username, password);
                break;
            default:
                return BadRequest("Invalid app code given");
        }
        
        // Invalid credentials
        if (!authenticated)
            return Unauthorized();
        
        // User was removed from database
         var user = await _authRepository.GetUser(username);
         if (user == null)
             return NotFound("User not found");
         
         // Generate token
         var token = await _authRepository.GenerateJwtToken(user);
         
         if (token == "")
             return Unauthorized("Invalid token");
         
        return Ok(token);
    }

    [HttpPost("Authorize")]
    public async Task<ActionResult<List<string>>> Authorize([FromQuery] string token, [FromQuery] string appCode)
    {
        if (token == null || token == "")
            return Unauthorized("No token provided.");

        var user = await _authRepository.ValidateJwtToken(token);
        if (user == null)
            return NotFound("Invalid token");
        
        var roleCodes = await _authRepository.GetUserRoles(user.Username, appCode);
        if (roleCodes == null || roleCodes.Count == 0)
            return NotFound("No roles found for this app's user");
        
        return Ok(roleCodes);
    }

}