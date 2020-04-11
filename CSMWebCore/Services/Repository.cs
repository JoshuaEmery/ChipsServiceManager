using CSMWebCore.Data;
using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public ChipsDbContext _db;

        public Repository(ChipsDbContext db)
        {
            // inject database context into repository
            _db = db;
        }
        
        public IEnumerable<T> GetAll() => _db.Set<T>().ToList();
        
        public IEnumerable<T> GetAll(Func<T, bool> exp) => _db.Set<T>().Where(exp).ToList();
        
        public T GetSingle(Func<T, bool> exp) => _db.Set<T>().Single(exp);

        public void Add(T entity) => _db.Add(entity);

        public int Commit() =>_db.SaveChanges();
    }
}
