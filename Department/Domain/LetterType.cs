using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LetterType
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Letter> Letters { get; set; } 
    }
}
