using CSMWebCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public ChipsDbContext _db;

        public Repository(ChipsDbContext db)
        {
            // inject database context into repository
            _db = db;
        }


        public void Add(TEntity entity) => _db.Add(entity);

        public int Commit() =>_db.SaveChanges();

        /// <summary>
        /// Gets all instances by getting a DbSet of the entity type 
        /// and creating a list from it.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll() => _db.Set<TEntity>().ToList();

        public IEnumerable<TEntity> Search(string searchValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a single entity that satisfies the function delegate expression.
        /// Throws an exception if more than one element exists.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public TEntity Single(Func<TEntity, bool> exp) => _db.Set<TEntity>().Single(exp);
    }
}
