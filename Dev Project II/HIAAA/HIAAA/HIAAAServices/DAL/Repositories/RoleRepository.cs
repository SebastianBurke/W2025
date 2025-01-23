using HIAAAServices.DAL.Interfaces;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices.DAL.Repositories;

public class RoleRepository : IRole
{
    private readonly Hia3Context _context;

    public RoleRepository(Hia3Context context)
    {
        _context = context;
    }

    public async Task<List<Role>> ReadAllRoles()
    {
        return await _context.Roles.Include(r => r.AppUserRoles).ThenInclude(aUR => aUR.App).ToListAsync();
    }

    public async Task<Role> ReadRole(long roleId)
    {
        return await _context.Roles.Include(r => r.AppUserRoles).ThenInclude(aUR => aUR.App).FirstOrDefaultAsync(r => r.Roleid == roleId);
    }

    public async Task<List<Role>> ReadAllAppRoles(int appId)
    {
        return await _context.Roles.Include(r => r.AppUserRoles).ThenInclude(aUR => aUR.App).Where(r => r.AppUserRoles.FirstOrDefault().Appid == appId).ToListAsync();
    }

    public async Task<List<Role>> GetUserAppRoles(string username, string appCode)
    {
        return await (from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            join role in _context.Roles on appUserRole.Roleid equals role.Roleid
            join app in _context.Apps on appUserRole.Appid equals app.Appid
            where user.Username == username && app.Appcode == appCode
            select role).ToListAsync();
    }

    public async Task<bool> CreateRole(Role newRole, long appId)
    {
        var app = await _context.Apps.FirstOrDefaultAsync(a => a.Appid == appId);
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Rolecode == $"{app.Appcode}_{newRole.Rolecode}");

        try
        {
            if (role != null) throw new Exception("Role already exists");

            newRole.Rolecode = $"{app.Appcode}_{newRole.Rolecode}";
            await _context.Roles.AddAsync(newRole);
            await _context.SaveChangesAsync();

            await _context.AppUserRoles.AddAsync(new AppUserRole()
                {
                    Roleid = newRole.Roleid,
                    Userid = null,
                    Appid = appId
                }
            );

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteRole(Role role)
    {
        if (role == null)
            return false;

        if (role.AppUserRoles.Any(aUR => aUR.User != null))
            return false;

        try
        {
            _context.AppUserRoles.RemoveRange(role.AppUserRoles);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateRole(Role updatedRole)
    {
        var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == updatedRole.Roleid);

        if (existingRole == null)
            return false;
        try
        {
            existingRole.Rolecode = $"{existingRole.AppUserRoles.FirstOrDefault().App.Appcode}_{updatedRole.Rolecode}";
            existingRole.Rolename = updatedRole.Rolename;
            existingRole.Roledescription = updatedRole.Roledescription;
            _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}