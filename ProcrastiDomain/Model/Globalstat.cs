using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Globalstat : Entity
{
    public int? Totallossamount { get; set; }

    public DateTime? Lastupdated { get; set; }
}
