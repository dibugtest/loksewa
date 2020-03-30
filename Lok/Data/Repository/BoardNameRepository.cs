using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class BoardNameRepository : Repository<BoardName>, IBoardNameRepository
    {
        public BoardNameRepository(IMangoContext context) : base(context)
        {
        }
    }
}
