using HIAAAServices.Models;

namespace HIAAAServices.DAL.Interfaces;

public interface IAuthRepository
{
    Task<int> GetAppTypeByCode(string appCode);
    Task<bool> AuthenticateLocally(string username, string password);
    Task<bool> AuthenticateHeritage(string username, string password);
    Task<bool> AuthenticateExternally(string username, string password);
    Task<User> GetUser(string username);
    Task<string> GenerateJwtToken(User user);
    Task<User> ValidateJwtToken(string token);
    Task<List<string>> GetUserRoles(string username, string appCode);
}