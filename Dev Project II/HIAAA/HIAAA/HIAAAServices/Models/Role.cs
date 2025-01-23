using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HIAAAServices.Models;

public partial class Role
{
    public long Roleid { get; set; }
    
    public string Rolecode { get; set; } = null!;

    public string Rolename { get; set; } = null!;
    
    public string? Roledescription { get; set; } = null!;
    
    public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();
}
