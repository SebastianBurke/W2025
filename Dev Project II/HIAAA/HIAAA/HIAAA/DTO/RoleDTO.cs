using System.ComponentModel.DataAnnotations;
using HIAAA.Models;

namespace HIAAA.DTO;

public class RoleDTO
{
    public long Roleid { get; set; }
    
    [Required(ErrorMessage = "Role code is required")]
    [UniqueRoleCode]
    public string Rolecode { get; set; } = null!;
    
    [Required(ErrorMessage = "Role name is required")]
    public string Rolename { get; set; } = null!;
    public long AppId { get; set; }
    
    [DataType(DataType.MultilineText)]
    public string? Roledescription { get; set; }
    
    public AppUserRole? AppUserRole { get; set; }
    
    public AppDTO App { get; set; } = null!;
    
    public RoleDTO() {}

    public RoleDTO(Role role)
    {
        Roleid = role.Roleid;
        AppId = role.AppUserRoles.FirstOrDefault()?.Appid ?? 0;
        Rolecode = role.Rolecode.Substring(role.Rolecode.IndexOf("_") + 1);
        Rolename = role.Rolename;
        Roledescription = role.Roledescription;
        App = new AppDTO()
        {
            AppId = role.AppUserRoles.FirstOrDefault().App.Appid,
            AppCode = role.AppUserRoles.FirstOrDefault().App.Appcode,
            AppName = role.AppUserRoles.FirstOrDefault().App.Appname,
            CreatedBy = role.AppUserRoles.FirstOrDefault().App.Createdby,
        };
    }
}