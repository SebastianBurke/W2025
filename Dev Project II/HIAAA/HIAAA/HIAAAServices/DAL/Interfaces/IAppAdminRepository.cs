using HIAAAServices.DTO;
using HIAAAServices.Models;

namespace HIAAAServices.DAL.Interfaces;

public interface IAppAdminRepository
{
    Task<List<User>> GetAllAppAdmins();
    Task<User> GetAppAdminById(long id);
    Task<User> GetAppAdminByUsername(string username);
    Task AddAppAdmin(UserDTO user);
    Task UpdateAppAdmin(UserDTO user);
    Task DeleteAppAdmin(long id);
}