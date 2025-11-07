using DataLayer.Contract;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intrfaces
{
    public interface IletterRepozitory :IGenericRepozitory<Letter>
    {
        IEnumerable<Letter> GetBySender(int senderId);
        IEnumerable<Letter> GetByReceiver(int receiverId);
        Letter? GetFullById(int id);
    }
}
