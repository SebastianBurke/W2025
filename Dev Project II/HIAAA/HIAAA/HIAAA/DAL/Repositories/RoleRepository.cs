using System.Text;
using HIAAA.DAL.Interfaces;
using HIAAA.DTO;
using HIAAA.Models;
using Newtonsoft.Json;

namespace HIAAA.DAL.Repositories;

public class RoleRepository : IRole
{
    private readonly HttpClient _httpClient;
    public RoleRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    
    public async Task<List<RoleDTO>> ReadAllRoles()
    {
        List<RoleDTO> roles = null;
        string endpoint = $"http://localhost:5264/Roles";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return new List<RoleDTO>();
        }

        string responseData = await response.Content.ReadAsStringAsync();
        roles = JsonConvert.DeserializeObject<List<RoleDTO>>(responseData);

        return roles;
    }

    public async Task<List<RoleDTO>> ReadAllAppRoles(int appId)
    {
        List<RoleDTO> roles = null;
        string endpoint = $"http://localhost:5264/Roles/Apps/{appId}";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return new List<RoleDTO>();
        }

        string responseData = await response.Content.ReadAsStringAsync();
        roles = JsonConvert.DeserializeObject<List<RoleDTO>>(responseData);

        return roles;
    }

    public async Task<RoleDTO> ReadRole(long roleId)
    {
        RoleDTO role = null;
        string endpoint = $"http://localhost:5264/Roles/{roleId}";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseData = await response.Content.ReadAsStringAsync();
        role = JsonConvert.DeserializeObject<RoleDTO>(responseData);
        role.Rolecode = role.Rolecode.Substring(role.Rolecode.IndexOf("_") + 1);
        return role;
    }
    
    public async Task<bool> CreateRole(long id, RoleDTO newRoleDTO)
    {
        string endpoint = $"http://localhost:5264/Roles";
        string jsonContent = JsonConvert.SerializeObject(newRoleDTO);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteRole(long id)
    {
        string endpoint = $"http://localhost:5264/Roles/{id}";
        HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateRole(RoleDTO updatedRoleDTO)
    {
        string endpoint = $"http://localhost:5264/Roles/{updatedRoleDTO.Roleid}";
        string jsonContent = JsonConvert.SerializeObject(updatedRoleDTO);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

        return response.IsSuccessStatusCode;
    }
}