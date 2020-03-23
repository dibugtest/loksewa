using Lok.Data.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMangoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected Repository(IMangoContext context)
        {
            Context = context;
        }

        public virtual void Add(TEntity obj)
        {
            ConfigDbSet();
            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        private void ConfigDbSet()
        {
            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async Task<TEntity> GetById(string id)
        {

            ConfigDbSet();
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id)));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            ConfigDbSet();
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj,string id)
        {
            ConfigDbSet();
            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id)), obj));
        }

        public virtual void Remove(string id)
        {
            ConfigDbSet();
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id))));
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}


        //public void Dispose()
        //{

        //    Context?.Dispose();
        //}

    }
}
