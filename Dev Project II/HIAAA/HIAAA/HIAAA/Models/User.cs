using System;
using System.Collections.Generic;

namespace HIAAA.Models;

public partial class User
{
    public long Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();

    public virtual ICollection<App> Apps { get; set; } = new List<App>();

    public virtual LocalUser? LocalUser { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
