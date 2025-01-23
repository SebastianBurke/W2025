using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Hia3Context _context;

    public UserRepository(Hia3Context context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers(long appId)
    {
        return await (from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            where appUserRole.Appid == appId
            select user).Distinct().ToListAsync();
    }
    
    public async Task<User> GetUserById(long id, long appId)
    {
        return await (from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            where user.Userid == id && appUserRole.Appid == appId
            select user).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByUsername(string username, long appId)
    {
        return await (from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            where user.Username == username && appUserRole.Appid == appId
            select user).FirstOrDefaultAsync();
    }
    
    public async Task AddUser(UserDTO userDto, long appId, long defaultRoleId)
    {
        // Check if the user already exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

        if (existingUser == null)
        {
            // Add the user to the Users table
            var newUser = new User
            {
                Username = userDto.Username,
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            existingUser = newUser;
        }

        // Check if the user is already associated with the app
        var appUserRole = await _context.AppUserRoles
            .FirstOrDefaultAsync(aur => aur.Userid == existingUser.Userid && aur.Appid == appId);

        if (appUserRole != null)
        {
            throw new InvalidOperationException("This user is already associated with the specified app.");
        }

        // Add an entry to the AppUserRoles table with the default role
        var appUserRoleEntry = new AppUserRole
        {
            Userid = existingUser.Userid,
            Appid = appId,
            Roleid = defaultRoleId // Explicit default role
        };

        await _context.AppUserRoles.AddAsync(appUserRoleEntry);
        await _context.SaveChangesAsync();
    }



    public async Task DeleteUser(long id, long appId)
    {
        // Fetch all associations for the user in the app
        var appUserRoles = _context.AppUserRoles.Where(aur => aur.Userid == id && aur.Appid == appId);

        if (!await appUserRoles.AnyAsync())
        {
            throw new KeyNotFoundException("User not found for the specified app.");
        }

        // Remove the app-user associations
        _context.AppUserRoles.RemoveRange(appUserRoles);

        // Check if the user has other app associations
        var hasOtherAssociations = await _context.AppUserRoles.AnyAsync(aur => aur.Userid == id);
        if (!hasOtherAssociations)
        {
            // Remove the user only if they have no other app associations
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateUser(UserDTO userDto, long appId)
    {
        // Check if the user exists
        var existingUser = await _context.Users.FindAsync(userDto.Userid);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        // Ensure the user is associated with the app
        var appUserRole = await _context.AppUserRoles
            .FirstOrDefaultAsync(aur => aur.Userid == userDto.Userid && aur.Appid == appId);

        if (appUserRole == null)
        {
            throw new InvalidOperationException("User is not associated with the specified app.");
        }

        // Update user details
        existingUser.Username = userDto.Username;
        existingUser.Firstname = userDto.Firstname;
        existingUser.Lastname = userDto.Lastname;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<User>> SearchUsersByName(string firstName, string lastName, long appId)
    {
        var query = from user in _context.Users
            join appUserRole in _context.AppUserRoles on user.Userid equals appUserRole.Userid
            where appUserRole.Appid == appId
            select user;

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(u => EF.Functions.Like(u.Firstname, $"%{firstName}%"));
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(u => EF.Functions.Like(u.Lastname, $"%{lastName}%"));
        }

        return await query.Distinct().ToListAsync();
    }

    public async Task AssignUsertoRole(string appCode, string username, string roleCode)
    {
        var app = await _context.Apps.FirstOrDefaultAsync(a => a.Appcode.ToLower() == appCode.ToLower());
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Rolecode.ToLower() == $"{app.Appcode}_{roleCode}".ToLower());

        if (user == null || role == null || app == null)
            throw new KeyNotFoundException($"Not found.");

        await _context.AppUserRoles.AddAsync(new AppUserRole() {Appid = app.Appid, Userid = user.Userid, Roleid = role.Roleid});
        await _context.SaveChangesAsync();
    }
}
