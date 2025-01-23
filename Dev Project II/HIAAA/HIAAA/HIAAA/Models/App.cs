using System;
using System.Collections.Generic;

namespace HIAAA.Models;

public partial class App
{
    public long Appid { get; set; }

    public string Appcode { get; set; } = null!;

    public string Appname { get; set; } = null!;
    
    public int Apptype { get; set; }

    public string? Appdescription { get; set; }

    public long Createdby { get; set; }

    public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();

    public virtual User CreatedbyNavigation { get; set; } = null!;
}
