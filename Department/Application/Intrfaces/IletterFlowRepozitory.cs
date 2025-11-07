using DataLayer.Contract;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intrfaces
{
    public interface IletterFlowRepozitory : IGenericRepozitory<LetterFlow>
    {
        IEnumerable<LetterFlow> GetFlowsByLetter(int letterId);
        IEnumerable<LetterFlow> GetFlowsByUser(int userId);
    }
}
