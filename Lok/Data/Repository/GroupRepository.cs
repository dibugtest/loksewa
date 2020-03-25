using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class GroupRepository:Repository<Group>,IGroupRepository
    {
        public GroupRepository(IMangoContext context) : base(context)
        {
        }

    }
}
