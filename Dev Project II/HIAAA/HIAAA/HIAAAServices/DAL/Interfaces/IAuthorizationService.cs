namespace HIAAAServices.DAL.Interfaces;

public interface IAuthorizationService
{
    Task<bool> IsAuthorizedForApp(long userId, long appId);
}
