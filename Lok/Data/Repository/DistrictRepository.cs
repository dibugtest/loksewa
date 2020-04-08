using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        public DistrictRepository(IMangoContext context) : base(context)
        {
        }
    }
}
