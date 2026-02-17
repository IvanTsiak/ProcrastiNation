using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Title : Entity
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool? Isunique { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Usertitle> Usertitles { get; set; } = new List<Usertitle>();
}
