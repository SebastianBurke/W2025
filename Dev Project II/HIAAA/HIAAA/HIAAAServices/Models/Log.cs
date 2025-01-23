using System;
using System.Collections.Generic;

namespace HIAAAServices.Models;

public partial class Log
{
    public long Logid { get; set; }

    public DateTime Date { get; set; }

    public string Logevent { get; set; } = null!;

    public long Userid { get; set; }

    public virtual User User { get; set; } = null!;
}
