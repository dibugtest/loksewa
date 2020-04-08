using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class VargaRepository : Repository<Varga>, IVargaRepository
    {
        public VargaRepository(IMangoContext context) : base(context)
        {
        }
    }
}
