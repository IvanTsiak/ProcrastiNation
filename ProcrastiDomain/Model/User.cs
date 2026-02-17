using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class User : Entity
{
    public int? Titleid { get; set; }

    public string? Username { get; set; }

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? Profilepicture { get; set; }

    public int? Totalloss { get; set; }

    public DateTime? Joineddate { get; set; }

    public bool? Isbanned { get; set; }

    public bool? Isadmin { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Log> LogsNavigation { get; set; } = new List<Log>();

    public virtual Title? Title { get; set; }

    public virtual ICollection<Userachievement> Userachievements { get; set; } = new List<Userachievement>();

    public virtual ICollection<Usertitle> Usertitles { get; set; } = new List<Usertitle>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
