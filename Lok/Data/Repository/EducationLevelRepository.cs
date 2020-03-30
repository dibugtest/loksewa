using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class EducationLevelRepository : Repository<EducationLevel>, IEducationLevelRepository
    {
        public EducationLevelRepository(IMangoContext context) : base(context)
        {
        }
    }
}
