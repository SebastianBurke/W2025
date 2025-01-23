using System.ComponentModel.DataAnnotations;
using HIAAAServices.Models;

namespace HIAAAServices.DTO;

public class RoleDTO
{
    public long Roleid { get; set; }
    
    [Required]
    [UniqueRoleCode]
    public string Rolecode { get; set; } = null!;
    
    [Required]
    public string Rolename { get; set; } = null!;
    
    public long AppId { get; set; }
    
    public string? Roledescription { get; set; } = null!;
    
    public AddAppDto App { get; set; } = null!;
    
    public RoleDTO() {}

    public RoleDTO(Role role)
    {
        Roleid = role.Roleid;
        AppId = role.AppUserRoles.FirstOrDefault()?.Appid ?? 0;
        Rolecode = role.Rolecode;
        Rolename = role.Rolename;
        Roledescription = role.Roledescription;
        App = new AddAppDto()
        {
            AppId = role.AppUserRoles.FirstOrDefault().App.Appid,
            AppCode = role.AppUserRoles.FirstOrDefault().App.Appcode,
            AppName = role.AppUserRoles.FirstOrDefault().App.Appname,
            CreatedBy = role.AppUserRoles.FirstOrDefault().App.Createdby,
        };
    }
}