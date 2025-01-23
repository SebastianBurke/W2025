using HIAAAServices.Models;

namespace HIAAAServices.DAL.Interfaces;

public interface IRole
{
    public Task<List<Role>> ReadAllRoles();

    public Task<Role> ReadRole(long roleId);

    public Task<List<Role>> ReadAllAppRoles(int appId);
    
    public Task<List<Role>> GetUserAppRoles(string username, string appCode);

    public Task<bool> CreateRole(Role newRole, long appId);
    
    public Task<bool> DeleteRole(Role role);
    
    public Task<bool> UpdateRole(Role updatedRole);
}