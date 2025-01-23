using HIAAAServices.DTO;
using HIAAAServices.Models;

namespace HIAAAServices.DAL.Interfaces;

public interface IApp
{
    // Retrieve all apps.
    Task<List<App>> GetAppsAsync(); 
    // Retrieve a specific app by ID.
    Task<App> GetAppByIdAsync(int id); 
    // Create a new app and assign an admin.
    Task<AddAppDto> AddAppAsync(AddAppDto dto);
    
    // Update app details, check permissions.
    Task<App> UpdateAppAsync(App app); 
    // Delete app, restricted to admins.
    Task<bool> DeleteAppAsync(int appId);
    // Retrieve apps associated with a specific user (optional).
    Task<List<App>> GetAppsByUserAsync(int userId);

    Task AssignAppToAppAdmin(int appId, int userId, int roleId);

    Task<List<User>> GetUsersByAppAdminRole(string roleCode);
}
