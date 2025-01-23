using System;
using System.Collections.Generic;

namespace TestScenario1.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public int? RoleId { get; set; }

    public bool? CanCreateApp { get; set; }

    public bool? CanCreateRole { get; set; }

    public bool? CanAddMembers { get; set; }

    public virtual Role? Role { get; set; }
}
