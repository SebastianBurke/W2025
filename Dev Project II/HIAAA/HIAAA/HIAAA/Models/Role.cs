using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HIAAA.Models;

public partial class Role
{
    public long Roleid { get; set; }

    [Required(ErrorMessage = "Role code is required")]
    [UniqueRoleCode(ErrorMessage = "Role code already exists within the application")]
    public string Rolecode { get; set; } = null!;

    [Required(ErrorMessage = "Role name is required")]
    public string Rolename { get; set; } = null!;
    
    [DataType(DataType.MultilineText)]
    public string Roledescription { get; set; } = null!;
    public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();
}
