using HIAAAServices.DTO;
using HIAAAServices.Models;

namespace HIAAAServices.DAL.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers(long appId);
    Task<User> GetUserById(long id, long appId);
    Task<User> GetUserByUsername(string username, long appId);
    Task AddUser(UserDTO userDto, long appId, long defaultRoleId);
    Task DeleteUser(long id, long appId);
    Task UpdateUser(UserDTO userDto, long appId);
    Task<List<User>> SearchUsersByName(string firstName, string lastName, long appId);
    Task AssignUsertoRole(string appCode, string username, string rolecode);
}