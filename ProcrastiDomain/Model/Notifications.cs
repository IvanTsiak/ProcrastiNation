using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProcrastiDomain.Model
{
    public partial class Notification : Entity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool Isviewed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Link { get; set; }
    }
}
