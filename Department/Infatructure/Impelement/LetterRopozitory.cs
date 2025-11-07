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
    public class LetterRopozitory : GenericRepository<Letter>, IletterRepozitory
    {
        private readonly ApplicationDbContext _context;
        public LetterRopozitory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Letter> GetBySender(int senderId)
        {
            return _context.Letters
                .Include(l => l.Receiver)
                .Include(l => l.LetterType)
                .Where(l => l.SenderId == senderId)
                .ToList();
        }

        public IEnumerable<Letter> GetByReceiver(int receiverId)
        {
            return _context.Letters
                .Include(l => l.Sender)
                .Include(l => l.LetterType)
                .Where(l => l.ReceiverId == receiverId)
                .ToList();
        }

        public Letter? GetFullById(int id)
        {
            return _context.Letters
                .Include(l => l.Sender)
                .Include(l => l.Receiver)
                .Include(l => l.LetterType)
                .Include(l => l.Notes)
                .FirstOrDefault(l => l.Id == id);
        }
    }
}
