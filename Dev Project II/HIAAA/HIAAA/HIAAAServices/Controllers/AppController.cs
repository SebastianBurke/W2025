using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIAAAServices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IApp _appRepository;
        
        public AppController(IApp appRepository)
        {
            _appRepository = appRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetApps()
        {
            return Ok(await _appRepository.GetAppsAsync());
        }
        
        [HttpGet("AppAdmin")]
        public async Task<ActionResult> GetUsersOfAppAdmin(string roleCode)
        {
            return Ok(await _appRepository.GetUsersByAppAdminRole(roleCode));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAppById(int id)
        {
            return Ok(await _appRepository.GetAppByIdAsync(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateApp([FromBody] AddAppDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appDto = await _appRepository.AddAppAsync(dto);

            return CreatedAtAction(nameof(GetAppById), new { id = appDto.AppId }, appDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApp(int id, [FromBody] UpdateAppDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var app = new App
            {
                Appid = id,
                Appcode = dto.AppCode,
                Apptype = dto.Apptype,
                Appname = dto.AppName,
                Appdescription = dto.Appdescription,
                Createdby = dto.CreatedBy
            };

            try
            {
                var updatedApp = await _appRepository.UpdateAppAsync(app);
                return Ok(updatedApp);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApp(int id)
        {
            try
            {
                var success = await _appRepository.DeleteAppAsync(id);
                if (success)
                {
                    return NoContent(); // 204 No Content for successful deletion
                }

                return BadRequest("Failed to delete the app.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden if user is unauthorized
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // 404 Not Found if app does not exist
            }
        }

        [HttpGet("user/{userId}/apps")]
        public async Task<IActionResult> GetAppsByUser(int userId)
        {
            try
            {
                var apps = await _appRepository.GetAppsByUserAsync(userId);
                if (apps == null || !apps.Any())
                {
                    return NotFound($"No apps found for user with ID {userId}.");
                }

                return Ok(apps);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost("{appId}/AssignAdmin")]
        public async Task<IActionResult> AssignAdmin(int appId, [FromBody] AssignAppAdminDto dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.RoleId <= 0)
            {
                return BadRequest("Invalid input.");
            }

            try
            {
                await _appRepository.AssignAppToAppAdmin(appId, dto.UserId, dto.RoleId);
                return Ok("App successfully assigned to admin.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




    }
}
