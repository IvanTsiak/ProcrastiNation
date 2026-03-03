using System.ComponentModel.DataAnnotations.Schema;

namespace ProcrastiDomain.Model
{
    // Одразу жорстко прив'язуємо до правильної таблиці, щоб уникнути сюрпризів
    [Table("likes")]
    public partial class Like
    {
        [Column("userid")]
        public int Userid { get; set; }

        [Column("logid")]
        public int Logid { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual Log Log { get; set; } = null!;
    }
}