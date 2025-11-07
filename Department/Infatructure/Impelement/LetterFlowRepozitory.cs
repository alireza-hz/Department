using Application.Intrfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infatructure.Impelement
{
    public class LetterFlowRepozitory : GenericRepository<LetterFlow>, IletterFlowRepozitory
    {
        private readonly ApplicationDbContext _context;
        public LetterFlowRepozitory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public IEnumerable<LetterFlow> GetFlowsByLetter(int letterId)
        {
            return _context.LetterFlows
                .Include(f => f.FromUser)
                .Include(f => f.ToUser)
                .Where(f => f.LetterId == letterId)
                .OrderByDescending(f => f.SentAt)
                .ToList();
        }

        public IEnumerable<LetterFlow> GetFlowsByUser(int userId)
        {
            return _context.LetterFlows
                .Include(f => f.Letter)
                .Where(f => f.ToUserId == userId || f.FromUserId == userId)
                .OrderByDescending(f => f.SentAt)
                .ToList();
        }
    }
}
