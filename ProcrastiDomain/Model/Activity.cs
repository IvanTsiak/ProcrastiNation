using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcrastiDomain.Model;

public partial class Activity : Entity
{
    public string Name { get; set; } = null!;

    public int? Categoryid { get; set; }

    public int? Mentionscount { get; set; }

    public virtual Category? Category { get; set; }

    [Column("isverified")]
    public bool Isverified { get; set; } = false;

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
