using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices.DAL.Repositories;

public class AppAdminRepository : IAppAdminRepository {
    
    private readonly Hia3Context _context;

    public AppAdminRepository(Hia3Context context)
    {
        _context = context;
    }
    
    public async Task<List<User>> GetAllAppAdmins()
    {
        var appAdmins = await (from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            join role in _context.Roles on appUserRole.Roleid equals role.Roleid
            where role.Rolecode == "APPADMIN"
            select user).ToListAsync();
        return appAdmins;
    }
    
    public async Task<User> GetAppAdminById(long id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetAppAdminByUsername(string username)
    {
        var appAdmin = await (from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            join role in _context.Roles on appUserRole.Roleid equals role.Roleid
            where role.Rolecode == "APPADMIN" && EF.Functions.Like(user.Username, username)
            select user).FirstOrDefaultAsync();
        return appAdmin;
    }

    
    public async Task AddAppAdmin(UserDTO user) {
        
        // check if already exists in the User table
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        var appAdminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Rolecode == "APPADMIN");
        if (existingUser == null)
        {
            await _context.Users.AddAsync(new User()
            {
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname
            });
            await _context.SaveChangesAsync();
        }
        else
        {
            var appAdmin =
                await _context.AppUserRoles.FirstOrDefaultAsync(aur =>
                    aur.Roleid == appAdminRole.Roleid && aur.Userid == existingUser.Userid);
            if (appAdmin != null)
            {
                throw new InvalidOperationException("This app admin already exists");
            }
        }
        
        // add to associative table
        var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        await _context.AppUserRoles.AddAsync(new AppUserRole()
        {
            Roleid = appAdminRole.Roleid,
            Userid = newUser.Userid,
            Appid = null
        });
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAppAdmin(long id)
    {
        // check if the user exists
        var appAdmin = await _context.Users.FindAsync(id);
        if (appAdmin == null)
        {
            throw new KeyNotFoundException("App admin not found.");
        }

        // check if there are any apps associated with this admin
        var hasApps = await _context.Apps
            .AnyAsync(a => a.Createdby == appAdmin.Userid);
        if (hasApps)
        {
            throw new InvalidOperationException("Cannot delete this app admin because they are associated with applications.");
        }

        var appUserRoles = _context.AppUserRoles.Where(aur => aur.Userid == id);
        _context.AppUserRoles.RemoveRange(appUserRoles);
        _context.Users.Remove(appAdmin);
        await _context.SaveChangesAsync();
    }

    
    public async Task UpdateAppAdmin(UserDTO user) {
        // check if the user exists
        var existingUser = await _context.Users.FindAsync(user.Userid);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("App admin not found.");
        }

        // update user details
        existingUser.Username = user.Username;
        existingUser.Firstname = user.Firstname;
        existingUser.Lastname = user.Lastname;

        _context.Users.Update(existingUser);

        await _context.SaveChangesAsync();
    }
}