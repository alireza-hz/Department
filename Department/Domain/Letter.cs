using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Letter
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public bool IsSecret { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; } 

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public int LetterTypeId { get; set; }
        public LetterType LetterType { get; set; }

        public string Status { get; set; }
        public ICollection<LetterFlow> LetterFlows { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Attachment> Attachments { get; set; } 
    }
}
