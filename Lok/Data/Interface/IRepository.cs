using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj,string id);
        void Remove(string id);
    }
}
