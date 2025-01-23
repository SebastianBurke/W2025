using HIAAAServices.DAL.Interfaces;
using HIAAAServices.Models;

namespace HIAAAServices.DAL.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly Hia3Context _context;

        public LoginRepository(Hia3Context context)
        {
            _context = context;
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }

        public LocalUser? GetLocalUserById(long localUserId)
        {
            return _context.LocalUsers.SingleOrDefault(lu => lu.Localuserid == localUserId);
        }

        public Role? GetRoleById(long roleId)
        {
            return _context.Roles.SingleOrDefault(r => r.Roleid == roleId);
        }

        public IEnumerable<Role> GetRolesByUserId(long userId)
        {
            var roleIds = _context.AppUserRoles
                .Where(aur => aur.Userid == userId)
                .Select(aur => aur.Roleid);
            return _context.Roles.Where(r => roleIds.Contains(r.Roleid)).ToList();
        }

        public IEnumerable<AppUserRole> GetAppUserRolesByAppCode(string appCode)
        {
            var appId = _context.Apps
                .Where(app => app.Appcode == appCode)
                .Select(app => app.Appid)
                .FirstOrDefault();

            return _context.AppUserRoles.Where(aur => aur.Appid == appId).ToList();
        }
    }
}