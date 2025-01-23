using HIAAA.DAL.Interfaces;
using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HIAAA.DAL.Repositories;

public class AppRepository : IApp
{
    private readonly HttpClient _httpClient;
    
    public AppRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<App> GetApp(int id)
    {
        App app = null;
        string endpoint = $"/App/{id}";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return app;
        }

        string responseData = response.Content.ReadAsStringAsync().Result;
        app = JsonConvert.DeserializeObject<App>(responseData);

        return app;
    }
    public async Task<List<App>> GetAppsAsync()
    {
        // Make an HTTP GET request to the API endpoint
        var response = await _httpClient.GetAsync("/app");

        // Check if the response is successful
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching apps: {response.StatusCode}");
        }

        // Deserialize the JSON response into a List<App>
        var apps = await response.Content.ReadFromJsonAsync<List<App>>();

        // Return the list of apps or an empty list if the response is null
        return apps ?? new List<App>();
    }

    public async Task<App> GetAppByIdAsync(int id)
    {
        // Construct the API endpoint URL
        var url = $"/app/{id}";

        // Send the GET request
        var response = await _httpClient.GetAsync(url);

        // Ensure the response indicates success
        if (response.IsSuccessStatusCode)
        {
            // Deserialize and return the app object
            return await response.Content.ReadFromJsonAsync<App>();
        }

        // Handle errors
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Failed to retrieve app with ID {id}: {errorContent}");
    }

    public Task<List<App>> GetAppsByUserAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<App> AddAppAsync(AddAppDTO dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/App", dto);

        if (response.IsSuccessStatusCode)
        {
            // Deserialize and return the created app
            return await response.Content.ReadFromJsonAsync<App>();
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Failed to create app: {errorContent}");
    }

    public async Task<App> UpdateAppAsync(UpdateAppDTO updateAppDto)
    {
        // Construct the API endpoint
        var url = $"/app/{updateAppDto.AppId}";
        Console.WriteLine($"Sending PUT request to: {url}");

        try
        {
            // Send the PUT request with the updateAppDto object as JSON
            var response = await _httpClient.PutAsJsonAsync(url, updateAppDto);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully updated app with ID: {updateAppDto.AppId}");
                // Deserialize and return the updated app
                return await response.Content.ReadFromJsonAsync<App>();
            }

            // Log and handle errors
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating app: {errorContent}");
            throw new HttpRequestException($"Failed to update app: {errorContent}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            throw;
        }
    }




    public async Task<bool> DeleteAppAsync(int appId)
    {
        if (appId <= 0)
        {
            throw new ArgumentException("Invalid app ID.", nameof(appId));
        }
        
        var url = $"/App/{appId}";

        try
        {
            var response = await _httpClient.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to delete app with ID {appId}: {errorContent}");
                throw new HttpRequestException($"Error deleting app: {response.StatusCode} - {errorContent}");
            }

            Console.WriteLine($"Successfully deleted app with ID {appId}");
            return true;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }



    public List<SelectListItem> GetAppTypeOptions()
    {
        // Simulate fetching from database or other source
        var appTypes = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Local Database" },
            new SelectListItem { Value = "2", Text = "Heritage College" },
            new SelectListItem { Value = "3", Text = "External Application" }
        };

        return appTypes;
    }
    
    public async Task AssignAppToAppAdmin(long appId, long adminUserId)
    {
        if (appId <= 0 || adminUserId <= 0)
        {
            throw new ArgumentException("App ID and Admin User ID must be greater than zero.");
        }
        
        var endpoint = $"/App/{appId}/AssignAdmin";
        
        var payload = new
        {
            AppId = appId,
            UserId = adminUserId,
            RoleId = 2
        };
        
        var response = await _httpClient.PostAsJsonAsync(endpoint, payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error assigning app to AppAdmin: {errorMessage}");
        }
    }
}