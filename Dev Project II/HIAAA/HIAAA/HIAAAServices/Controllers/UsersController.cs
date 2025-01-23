using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DAL.Services;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace HIAAAServices.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IAuthorizationService _authService;
    private readonly IConfiguration _config;

    public UsersController(IUserRepository userRepo, IAuthorizationService authService, IConfiguration config)
    {
        _userRepo = userRepo;
        _authService = authService;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] long appId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            // Validate if the user is authorized for the app
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to access this app.");
            }

            // Fetch all users for the app
            var users = await _userRepo.GetAllUsers(appId);

            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUserById(long id, [FromQuery] long appId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            // Validate authorization
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to access this app.");
            }

            // Fetch user for the app
            var user = await _userRepo.GetUserById(id, appId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username, [FromQuery] long appId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            // Validate if the user is authorized for the app
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to access this app.");
            }

            // Fetch the user
            var user = await _userRepo.GetUserByUsername(username, appId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDTO user, [FromQuery] long appId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var currentUserId = GetCurrentUserId();

            // Validate if the user is authorized for the app
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to create users for this app.");
            }

            var defaultRoleName = _config.GetValue<string>("DefaultRole:Name");
            // var defaultRole = await _roleRepo.GetRoleByName(defaultRoleName);
            // if (defaultRole == null)
            // {
            //     throw new Exception($"Default role '{defaultRoleName}' is not configured.");
            // }

            // Create the user and associate them with the app using the system-wide default role
            // await _userRepo.AddUser(user, appId, defaultRole.Roleid);

            return Ok("User created and associated with app successfully.");
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateUser(long id, UserDTO user, [FromQuery] long appId)
    {
        if (!ModelState.IsValid || id != user.Userid)
        {
            return BadRequest("Invalid request");
        }

        try
        {
            var currentUserId = GetCurrentUserId();

            // Validate if the user is authorized for the app
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to update users for this app.");
            }

            // Update the user
            await _userRepo.UpdateUser(user, appId);

            return Ok("User updated and associated with app successfully.");
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteUser(long id, [FromQuery] long appId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            // Validate if the user is authorized for the app
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to delete users for this app.");
            }

            // Delete the user
            await _userRepo.DeleteUser(id, appId);

            return Ok("User deleted successfully");
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsersByName([FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] long appId)
    {
        try
        {
            var currentUserId = GetCurrentUserId(); // Fetch logged-in user's ID

            if (appId <= 0)
            {
                return BadRequest("AppId is required.");
            }

            // Validate if the user is authorized for the app
            if (!await _authService.IsAuthorizedForApp(currentUserId, appId))
            {
                return Forbid("You are not authorized to access this app.");
            }

            // Call repository method
            var users = await _userRepo.SearchUsersByName(firstName, lastName, appId);

            if (users == null || !users.Any())
            {
                return NotFound("No users found for the specified app and name.");
            }

            return Ok(users);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    private long GetCurrentUserId()
    {
        // Fetch logged-in user's ID from the claims or session
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid or missing UserId claim.");
        }
        return userId;
    }

    // [HttpGet("")]
    // public async Task<IActionResult> AssignUsersToRoles()
    // {
    //     
    // }
}
