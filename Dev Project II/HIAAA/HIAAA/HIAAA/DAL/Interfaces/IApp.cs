using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HIAAA.DAL.Interfaces;

public interface IApp
{
    public Task<App> GetApp(int id);
    Task<List<App>> GetAppsAsync();

    // Get a specific app by its ID
    Task<App> GetAppByIdAsync(int id);

    // Get apps associated with a specific user
    Task<List<App>> GetAppsByUserAsync(int userId);

    // Add a new app
    Task<App> AddAppAsync(AddAppDTO dto);

    // Update an existing app
    Task<App> UpdateAppAsync(UpdateAppDTO updateAppDto);

    // Delete an app by its ID
    Task<bool> DeleteAppAsync(int appId);

    List<SelectListItem> GetAppTypeOptions();

    Task AssignAppToAppAdmin(long appId, long adminUserId);
}