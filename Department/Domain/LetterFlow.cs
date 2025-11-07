using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LetterFlow
    {
        public int Id { get; set; }

        public int LetterId { get; set; }
        public Letter Letter { get; set; }

        public int FromUserId { get; set; }
        public User FromUser { get; set; }
        public int ToUserId { get; set; }
        public User ToUser { get; set; } 

        public DateTime SentAt { get; set; } 
        public string Status { get; set; } 
    }
}
