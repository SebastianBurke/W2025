using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace HIAAAServices.Controllers;

[ApiController]
[Route("[controller]")]
public class AppAdminsController : ControllerBase
{
    private readonly IAppAdminRepository _appAdminRepo;

    public AppAdminsController(IAppAdminRepository appAdminRepo)
    {
        _appAdminRepo = appAdminRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAppAdmins()
    {
        try
        {
            var appAdmins = await _appAdminRepo.GetAllAppAdmins();
            return Ok(appAdmins);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAppAdminById(long id)
    {
        try
        {
            var appAdmin = await _appAdminRepo.GetAppAdminById(id);
            if (appAdmin == null)
                return NotFound("App admin not found");
            return Ok(appAdmin);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetAppAdminByUsername(string username)
    {
        try
        {
            var appAdmin = await _appAdminRepo.GetAppAdminByUsername(username);
            if (appAdmin == null)
                return NotFound("App admin not found");
            return Ok(appAdmin);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppAdmin(UserDTO user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _appAdminRepo.AddAppAdmin(user);
            return Ok();
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
    public async Task<IActionResult> UpdateAppAdmin(int id, UserDTO user)
    {
        if (!ModelState.IsValid || id != user.Userid)
        {
            return BadRequest("Invalid request");
        }

        try
        {
            await _appAdminRepo.UpdateAppAdmin(user);
            return Ok("App admin updated successfully");
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppAdmin(long id)
    {
        try
        {
            await _appAdminRepo.DeleteAppAdmin(id);
            return Ok("App admin deleted");
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
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
}