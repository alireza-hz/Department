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
    public class NoteRepozitory : GenericRepository<Note>, INoteRepozitory
    {
        private readonly ApplicationDbContext _context;
        public NoteRepozitory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Note> GetNotesByLetter(int letterId)
        {
            return _context.Notes
                .Include(n => n.User)
                .Where(n => n.LetterId == letterId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public IEnumerable<Note> GetNotesByUser(int userId)
        {
            return _context.Notes
                .Include(n => n.Letter)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }
    }
}
