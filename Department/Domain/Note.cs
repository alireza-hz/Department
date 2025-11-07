using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Note
    {
        public int Id { get; set; }

        public string Text { get; set; } 
        public DateTime CreatedAt { get; set; }

        public int LetterId { get; set; }
        public Letter Letter { get; set; } = default!;

        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
