using System;
using System.Collections.Generic;

namespace TestScenario1.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
