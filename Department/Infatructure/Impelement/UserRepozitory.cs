
using Application.Intrfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infatructure.Impelement
{
    public class UserRepozitory : GenericRepository<User>, IUserRepozitory
    {
        private readonly ApplicationDbContext _context;

        public UserRepozitory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
