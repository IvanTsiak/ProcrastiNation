using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Category : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
