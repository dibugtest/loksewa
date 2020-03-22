using Lok.Data.Interface;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Lok.Data.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly IMangoContext _context;
        private Disposable _myobject;
        public UnitOfWork(IMangoContext context, Disposable myObject)
        {
            _context = context;
            _myobject = myObject;
        }

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _myobject?.Dispose();


        }
    }
}

