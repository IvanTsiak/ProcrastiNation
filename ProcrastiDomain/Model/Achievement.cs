using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Achievement : Entity
{
    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public bool? Ishidden { get; set; }

    public virtual ICollection<Userachievement> Userachievements { get; set; } = new List<Userachievement>();
}
