using LoginAPI.Models;

namespace LoginAPI.DAL {
    public interface IAuthRepository {

        bool Authenticated(string username, string password);

        string GenerateJwtToken(User user, byte[] key);
        
        User GetUser(string username);
        
        Task<List<UserRole>> GetUserRoles(int userId);
    }
}
