using System;
using System.Collections.Generic;

namespace ProcrastiDomain.Model;

public partial class Comment : Entity
{
    public int? Logid { get; set; }

    public int? Authorid { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public int? Parentcommentid { get; set; }

    public virtual User? Author { get; set; }

    public virtual ICollection<Comment> InverseParentcomment { get; set; } = new List<Comment>();

    public virtual Log? Log { get; set; }

    public virtual Comment? Parentcomment { get; set; }
}
