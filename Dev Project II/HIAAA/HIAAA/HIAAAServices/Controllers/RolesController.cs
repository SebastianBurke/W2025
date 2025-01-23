using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace HIAAAServices.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : Controller
{
    private readonly IRole _roleRepository;
    private readonly IApp _appRepository;

    public RolesController(IRole roleRepository, IApp appRepository)
    {
        _roleRepository = roleRepository;
        _appRepository = appRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<RoleDTO>>> GetRoles()
    {
        var allRoles = await _roleRepository.ReadAllRoles();
        return Ok(allRoles.Select(r => new RoleDTO(r)).ToList());
    }

    [HttpGet("Apps/{id}")]
    public async Task<ActionResult<List<RoleDTO>>> GetRolesByApp(int id)
    {
        var app = await _appRepository.GetAppByIdAsync(id);
        if (app == null)
            return NotFound();
        
        return Ok((await _roleRepository.ReadAllAppRoles(id)).Select(r => new RoleDTO(r)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDTO>> GetRole(long id)
    {
        var role = await _roleRepository.ReadRole(id);
        if (role == null)
            return NotFound();
        
        return new RoleDTO(role);
    }

    [HttpPost]
    public async Task<ActionResult> PostRole(RoleDTO newRoleDTO) {
        if (!ModelState.IsValid) return BadRequest("Bad Request");

        var newRole = new Role()
        {
            Roleid = newRoleDTO.Roleid,
            Rolename = newRoleDTO.Rolename,
            Rolecode = newRoleDTO.Rolecode,
            Roledescription = newRoleDTO.Roledescription,
        };
        
        if (!await _roleRepository.CreateRole(newRole, newRoleDTO.AppId))
            return BadRequest("Error Occured While Creating New Role");

        return CreatedAtAction(nameof(PostRole), new { id = newRoleDTO.Roleid }, newRoleDTO);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRole(long id)
    {
        var role = await _roleRepository.ReadRole(id);
        
        if (role == null)
            return NotFound();
        
        if (!await _roleRepository.DeleteRole(role))
            return BadRequest();

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutRole(RoleDTO updatedRoleDTO)
    {
        if (!ModelState.IsValid) return BadRequest("Bad Request");
        
        var updatedRole = new Role()
        {
            Roleid = updatedRoleDTO.Roleid,
            Rolename = updatedRoleDTO.Rolename,
            Rolecode = updatedRoleDTO.Rolecode,
            Roledescription = updatedRoleDTO.Roledescription,
        };
        
        if (!await _roleRepository.UpdateRole(updatedRole))
            return BadRequest("Error Occured While Updating Role");

        return Ok();
    }
}