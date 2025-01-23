using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoginAPI.Models;

public partial class UserRole
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int AppId { get; set; }

    [JsonIgnore]
    public virtual Application App { get; set; } = null!;
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
