using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Interface
{
    public interface IUnitOfWork:IDisposable
    {
       
        Task<bool> Commit();

    }
}
