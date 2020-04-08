using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class ShreniTahaRepository : Repository<ShreniTaha>, IShreniTahaRepository
    {
        public ShreniTahaRepository(IMangoContext context) : base(context)
        {
        }
    }
}
