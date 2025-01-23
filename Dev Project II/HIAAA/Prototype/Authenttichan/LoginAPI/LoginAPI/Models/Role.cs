using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoginAPI.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int? AppId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Application? App { get; set; }

    [JsonIgnore]
    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    [JsonIgnore]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
