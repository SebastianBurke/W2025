using System;
using System.Collections.Generic;

namespace LoginAPI.Models;

public partial class Application
{
    public int AppId { get; set; }

    public string AppName { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
