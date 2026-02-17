using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Usertitle
{
    public int Userid { get; set; }

    public int Titleid { get; set; }

    public DateTime? Unlockedat { get; set; }

    public virtual Title Title { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
