using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Return all instances of type T.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Return a single instance of an entity of type T with matching Id.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        T Single(Func<T, bool> exp);

        /// <summary>
        /// Tell database context to track entity for later insertion.
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Save changes made to database context to database.
        /// </summary>
        /// <returns>Number of state entries for entities/relationships
        /// written to database (used for many-to-many relationships).</returns>
        int Commit();
        
        /// <summary>
        /// Search relevant fields of entity for given string value and
        /// returns collection of entities with a match.
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        IEnumerable<T> Search(string searchValue);
    }
}
