using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataAccessLayer
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
