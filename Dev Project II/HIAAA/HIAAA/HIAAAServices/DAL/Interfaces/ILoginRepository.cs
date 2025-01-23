using HIAAAServices.Models;

namespace HIAAAServices.DAL.Interfaces
{
    public interface ILoginRepository
    {
        User? GetUserByUsername(string username);
        LocalUser? GetLocalUserById(long localUserId);
        Role? GetRoleById(long roleId);
        IEnumerable<Role> GetRolesByUserId(long userId);
        IEnumerable<AppUserRole> GetAppUserRolesByAppCode(string appCode);
    }
}