using DataLayer.Contract;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intrfaces
{
    public interface IUserRepozitory : IGenericRepozitory<User>
    {
        User GetByEmail(string email);
    }
}
