using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices.DAL.Repositories;

public class AppRepository: IApp
{
    private readonly Hia3Context _context;
    
    public AppRepository(Hia3Context context)
    {
        _context = context;
    }
    
    public Task<List<App>> GetAppsAsync()
    {
        var apps = _context.Apps.ToListAsync();
        return apps;
    }

    public Task<App> GetAppByIdAsync(int id)
    {
        var app = _context.Apps.FirstOrDefaultAsync(a => a.Appid == id);
        if (app == null)
        {
            throw new Exception("App not found");
        }
        return app;
    }
    
   public async Task<AddAppDto> AddAppAsync(AddAppDto dto)
    {
        // check if the app code is unique
        var isAppCodeUnique = await _context.Apps
            .AllAsync(a => a.Appcode != dto.AppCode);
        
        if (!isAppCodeUnique) {
            throw new ArgumentException("App code must be unique.");
        }
        
        // Create the app
        var app = new App
        {
            Appcode = dto.AppCode,
            Appname = dto.AppName,
            Appdescription = dto.Appdescription,
            Apptype = dto.Apptype,
            Createdby = dto.CreatedBy
        };
        _context.Apps.Add(app);
        await _context.SaveChangesAsync(); // Save to get AppId

        // Map the App entity to AppDto
        var appDto = new AddAppDto
        {
            AppId = app.Appid,
            AppCode = app.Appcode,
            AppName = app.Appname,
            Appdescription = app.Appdescription,
            Apptype = app.Apptype,
            CreatedBy = app.Createdby
        };

        return appDto;
    }

    public async Task<App> UpdateAppAsync(App app)
    {
        // // Check if the user is authorized (either Admin or App Admin for this app)
        // var isAuthorized = await _context.AppUserRoles
        //     .AnyAsync(aur => aur.Userid == updatedByUserId &&
        //                      aur.Appid == app.Appid &&
        //                      (aur.Role.Rolecode == "ADMIN" || aur.Role.Rolecode == "APPADMIN"));

        // if (!isAuthorized)
        // {
        //     throw new UnauthorizedAccessException("You are not authorized to update this application.");
        // }

        // Retrieve the existing app from the database
        var existingApp = await _context.Apps
            .FirstOrDefaultAsync(a => a.Appid == app.Appid);

        if (existingApp == null)
        {
            throw new ArgumentException($"App with ID {app.Appid} does not exist.");
        }
        
        // Update the app's properties
        existingApp.Appid = app.Appid;
        existingApp.Appcode = app.Appcode ?? existingApp.Appcode;
        existingApp.Createdby = app.Createdby;
        existingApp.Apptype = app.Apptype;
        existingApp.Appname = app.Appname ?? existingApp.Appname;
        existingApp.Appdescription = app.Appdescription ?? existingApp.Appdescription;

        // Save changes to the database
        _context.Apps.Update(existingApp);
        await _context.SaveChangesAsync();

        return existingApp;
    }

    public async Task<bool> DeleteAppAsync(int appId)
    {

        // Retrieve the app from the database
        var app = await _context.Apps
            .Include(a => a.AppUserRoles) // Include related AppUserRoles
            .FirstOrDefaultAsync(a => a.Appid == appId);

        if (app == null)
        {
            throw new ArgumentException($"App with ID {appId} does not exist.");
        }

        // Delete related AppUserRoles
        if (app.AppUserRoles != null && app.AppUserRoles.Any())
        {
            _context.AppUserRoles.RemoveRange(app.AppUserRoles);
        }

        // Delete the app
        _context.Apps.Remove(app);

        // Save changes
        await _context.SaveChangesAsync();

        return true;
    }
    

    public async Task<List<App>> GetAppsByUserAsync(int userId)
    {
        // Validate that the user exists
        var userExists = await _context.Users.AnyAsync(u => u.Userid == userId);
        if (!userExists)
        {
            throw new ArgumentException($"User with ID {userId} does not exist.");
        }

        // Fetch apps associated with the user
        var apps = await _context.AppUserRoles
            .Where(aur => aur.Userid == userId)
            .Select(aur => aur.App)            
            .Distinct()                       
            .ToListAsync();

        return apps;
    }
    
    public async Task AssignAppToAppAdmin(int appId, int userId, int roleId)
    {
        if (appId <= 0 || userId <= 0 || roleId <= 0)
        {
            throw new ArgumentException("App ID, User ID, and Role ID must be greater than zero.");
        }

        // Check if the AppUserRole already exists to avoid duplicates
        var existingAssignment = await _context.AppUserRoles
            .FirstOrDefaultAsync(aur => aur.Appid == appId && aur.Userid == userId && aur.Roleid == roleId);

        if (existingAssignment != null)
        {
            throw new InvalidOperationException("This user is already assigned as an admin for the specified app.");
        }

        // Create a new AppUserRole entry
        var appUserRole = new AppUserRole
        {
            Appid = appId,
            Userid = userId,
            Roleid = roleId
        };

        // Add and save changes
        _context.AppUserRoles.Add(appUserRole);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetUsersByAppAdminRole(string roleCode)
    {
        return await _context.Users
            .Include(u => u.AppUserRoles)
            .ThenInclude(aUR => aUR.Role)
            .Where(u => u.AppUserRoles.Any(aUR => aUR.Role.Rolecode == roleCode))
            .ToListAsync();
    }

}