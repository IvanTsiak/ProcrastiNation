using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Userachievement
{
    public int Userid { get; set; }

    public int Achievementid { get; set; }

    public DateTime? Unlockedat { get; set; }

    public virtual Achievement Achievement { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
