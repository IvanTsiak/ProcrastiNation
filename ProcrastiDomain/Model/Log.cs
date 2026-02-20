using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcrastiDomain.Model;

public partial class Log : Entity
{
    public int? Userid { get; set; }

    public int? Activityid { get; set; }

    [Column("logtype")]
    public LogType Logtype { get; set; }

    public int Amount { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Createdat { get; set; }

    public bool Isvisible { get; set; }

    public int? Likescount { get; set; }

    public virtual Activity? Activity { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? User { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
