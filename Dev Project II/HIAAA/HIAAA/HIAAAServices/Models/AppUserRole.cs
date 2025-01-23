using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HIAAAServices.Models;

public partial class AppUserRole
{
    public long Appuserroleid { get; set; }

    public long? Appid { get; set; }

    public long Roleid { get; set; }

    public long? Userid { get; set; }
    
    public virtual App? App { get; set; } = null!;

    [JsonIgnore]
    public virtual Role? Role { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
