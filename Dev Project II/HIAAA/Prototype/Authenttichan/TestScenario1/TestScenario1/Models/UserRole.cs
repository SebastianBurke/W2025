using System;
using System.Collections.Generic;

namespace TestScenario1.Models;

public partial class UserRole
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int AppId { get; set; }

    public virtual Application App { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
