using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Activity : Entity
{
    public string Name { get; set; } = null!;

    public int? Categoryid { get; set; }

    public int? Mentionscount { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
