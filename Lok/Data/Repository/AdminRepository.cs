using Lok.Data.Interface;
using Lok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class AdminRepository:Repository<Admins>,IAdminRepository
    {
        public AdminRepository(IMangoContext context) : base(context)
        {

        }
    }
}
