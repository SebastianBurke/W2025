using HIAAA.DTO;
using HIAAA.Models;

namespace HIAAA.DAL.Interfaces;

public interface IRole
{
    public Task<List<RoleDTO>> ReadAllRoles();
    public Task<List<RoleDTO>> ReadAllAppRoles(int appId);
    public Task<RoleDTO> ReadRole(long roleId);
    public Task<bool> CreateRole(long id, RoleDTO newRoleDTO);
    public Task<bool> DeleteRole(long id);
    public Task<bool> UpdateRole(RoleDTO updatedRoleDTO);
}