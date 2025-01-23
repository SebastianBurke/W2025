using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoginAPI.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public int? RoleId { get; set; }

    public bool? CanCreateApp { get; set; }

    public bool? CanCreateRole { get; set; }

    public bool? CanAddMembers { get; set; }

    [JsonIgnore]
    public virtual Role? Role { get; set; }
}
