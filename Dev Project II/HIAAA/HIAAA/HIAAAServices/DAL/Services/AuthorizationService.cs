using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DTO;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices.DAL.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly Hia3Context _context;

    public AuthorizationService(Hia3Context context)
    {
        _context = context;
    }

    public async Task<bool> IsAuthorizedForApp(long userId, long appId)
    {
        // Check if the user is an AppAdmin for the given AppId
        var isAuthorized = await _context.AppUserRoles.AnyAsync(aur =>
            aur.Userid == userId &&
            aur.Appid == appId &&
            aur.Roleid != null && // Ensure it's not a null role (indicating app association only)
            aur.Roleid == (from role in _context.Roles where role.Rolecode == "APPADMIN" select role.Roleid).FirstOrDefault()
        );

        return isAuthorized;
    }
}
