using HIAAA.Models;

namespace HIAAA.DTO;

public class AppAdminDTO
{
    public long Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;
    
    public AppAdminDTO () {}

    public AppAdminDTO(User user)
    {
        Userid = user.Userid;
        Username = user.Username;
        Firstname = user.Firstname;
        Lastname = user.Lastname;
    }
}