using Application.Intrfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infatructure.Impelement
{
    public class AttachmentRepository :  GenericRepository<Attachment> , IAttachmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AttachmentRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }
        public IEnumerable<Attachment> GetByLetterId(int letterId)
        {
            return _context.Attachments
                .Where(a => a.LetterId == letterId)
                .OrderByDescending(a => a.UploadedAt)
                .ToList();
        }
    }
}
