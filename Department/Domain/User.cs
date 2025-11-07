using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }    
        public ICollection<Letter> SentLetters { get; set; } 
        public ICollection<Letter> ReceivedLetters { get; set; }
        public ICollection<Note> Notes { get; set; }

    }
}
