using HIAAA.DTO;
using HIAAA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HIAAA.DAL;

public interface IAppAdminRepository
{
    Task<List<User>> GetAll();
    Task<User> GetById(long id);
    Task<List<User>> GetUsersOfAppAdmin(string appAdmin);
    Task<List<SelectListItem>> GetAppAdminsAsSelectListItems();
    Task<User> GetByUsername(string username);
    Task<bool> Add(User user);
    Task Update(AppAdminDTO appAdmin);
    Task SetPassword(AppAdminDTO appAdmin, string password);
    Task Delete(long id);
}