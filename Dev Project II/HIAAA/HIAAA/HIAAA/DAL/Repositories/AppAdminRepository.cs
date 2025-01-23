using System.Text;
using HIAAA.Areas.Identity.Data;
using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace HIAAA.DAL.Repositories;

public class AppAdminRepository : IAppAdminRepository
{
    private readonly HttpClient _httpClient;
    private readonly UserManager<HIAAAUser> _userManager;
    public AppAdminRepository(HttpClient httpClient, UserManager<HIAAAUser> userManager)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _userManager = userManager;
    }
    
    public async Task<List<User>> GetAll()
    {
        string endpoint = "/AppAdmins";
        HttpResponseMessage res = await _httpClient.GetAsync(endpoint);
        if (res.IsSuccessStatusCode)
        {
            string data = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<User>>(data);
        }
        return new List<User>();
    }

    public async Task<User> GetById(long id)
    {
        string endpoint = $"/AppAdmins/{id}";
        HttpResponseMessage res = await _httpClient.GetAsync(endpoint);
        if (res.IsSuccessStatusCode)
        {
            string data = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(data);
        }
        return null;
    }

    public async Task<User> GetByUsername(string username)
    {
        string endpoint = $"/AppAdmins/username/{username}";
        HttpResponseMessage res = await _httpClient.GetAsync(endpoint);
        if (res.IsSuccessStatusCode)
        {
            string data = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(data);
        }
        return null;
    }

    public async Task<bool> Add(User user)
    {
        string endpoint = "/AppAdmins";
        string jsonContent = JsonConvert.SerializeObject(user);
        StringContent content = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage res = await _httpClient.PostAsync(endpoint, content);
        return res.IsSuccessStatusCode;
    }

    public async Task Update(AppAdminDTO appAdmin)
    {
        var user = await GetById(appAdmin.Userid);
        user.Firstname = appAdmin.Firstname;
        user.Lastname = appAdmin.Lastname;
        string endpoint = $"/AppAdmins/{user.Userid}";
        string jsonContent = JsonConvert.SerializeObject(user);
        StringContent content = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage res = await _httpClient.PutAsync(endpoint, content);
        res.EnsureSuccessStatusCode();
    }

    public async Task SetPassword(AppAdminDTO appAdmin, string password)
    {
        // TODO: implement change username and/or password for app admins
    }

    public async Task Delete(long id)
    {
        var appAdmin = await GetById(id);
        if (appAdmin == null)
            throw new ApplicationException($"User with id {id} not found");
        string endpoint = $"/AppAdmins/{id}";
        HttpResponseMessage res = await _httpClient.DeleteAsync(endpoint);
        if (res.IsSuccessStatusCode)
        {
            var user = await _userManager.FindByEmailAsync(appAdmin.Username);
            if (user == null)
                throw new ApplicationException($"User with id {id} not found");
            
            await _userManager.RemoveFromRoleAsync(user, "APPADMIN");
            await _userManager.DeleteAsync(user);
        }
        else
        {
            throw new InvalidOperationException("Cannot delete this app admin because they are associated with applications.");
        }
    }
    
    public async Task<List<User>> GetUsersOfAppAdmin(string appAdmin)
    {
        // Make an HTTP GET request to the API endpoint
        var response = await _httpClient.GetAsync($"/app/AppAdmin?roleCode={appAdmin}");

        // Check if the response is successful
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching apps: {response.StatusCode}");
        }

        // Deserialize the JSON response into a List<App>
        var apps = await response.Content.ReadFromJsonAsync<List<User>>();

        // Return the list of apps or an empty list if the response is null
        return apps ?? new List<User>();
    }
    
    public async Task<List<SelectListItem>> GetAppAdminsAsSelectListItems()
    {
        var appAdmins = await GetUsersOfAppAdmin("APPADMIN"); // Fetch all AppAdmins
        return appAdmins.Select(admin => new SelectListItem
        {
            Value = admin.Userid.ToString(),
            Text = $"{admin.Firstname} {admin.Lastname} ({admin.Username})"
        }).ToList();
    }

}