using System;
using System.Collections.Generic;

namespace HIAAAServices.Models;

public partial class LocalUser
{
    public long Localuserid { get; set; }

    public string Password { get; set; } = null!;

    public virtual User Localuser { get; set; } = null!;
}
