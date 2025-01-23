using System;
using System.Collections.Generic;

namespace HIAAA.Models;

public partial class AppUserRole
{
    public long Appuserroleid { get; set; }

    public long? Appid { get; set; }

    public long? Roleid { get; set; }

    public long? Userid { get; set; }

    public virtual App? App { get; set; } = null!;

    public virtual Role? Role { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
