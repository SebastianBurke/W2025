using HIAAAServices.DAL.Interfaces;
using System.Linq;

namespace HIAAAServices.Controllers
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public AuthenticateResponse Authenticate(Authenticate request)
        {
            var user = _loginRepository.GetUserByUsername(request.Username);
            if (user == null)
                return new AuthenticateResponse { AuthenticateResult = false };

            var localUser = _loginRepository.GetLocalUserById(user.Userid);
            
            return new AuthenticateResponse
            {
                AuthenticateResult = localUser != null && localUser.Password == request.Password
            };
        }

        public AuthorizeResponse Authorize(Authorize request)
        {
            var user = _loginRepository.GetUserByUsername(request.Username);
            if (user == null)
                return new AuthorizeResponse
                {
                    AuthorizeResult = new UserBLL
                    {
                        HasError = true,
                        ErrorMessage = "User not found"
                    }
                };
            

            var roles = _loginRepository.GetAppUserRolesByAppCode(request.AppCode)
                .Where(aur => aur.Userid == user.Userid)
                .Select(aur => _loginRepository.GetRoleById(aur.Roleid))
                .Where(role => role != null)
                .Select(role => new RoleBLL
                {
                    RoleId = role.Roleid,
                    Code = role.Rolecode,
                    Description = role.Rolename
                })
                .ToArray();

            return new AuthorizeResponse
            {
                AuthorizeResult = new UserBLL
                {
                    UserId = user.Userid,
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Username = user.Username,
                    RoleList = roles,
                    HasError = false
                }
            };
        }
    }
}
