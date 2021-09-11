using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ClientBuisnessLogicLayer
{
    public interface IBaseService<T> where T : class
    {
        string Insert(T entity);
        string Update(T entity);
        string Delete(T entity);
        T SelectById(int? id);
        IEnumerable<T> SelectAll();
        bool Validate(T entity, out string message);
    }
}
