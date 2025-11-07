using Application.Intrfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infatructure.Impelement
{
    public class LetterTypeRepository : GenericRepository<LetterType>, ILetterTypeRepository
    {
        public LetterTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
