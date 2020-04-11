using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets a list of all instances of a type.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();


        /// <summary>
        /// Gets a list of all instances of a type that satisfy the expression.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll(Func<T, bool> exp);

        /// <summary>
        /// Gets a single entity that satisfies the expression.
        /// Throws an exception if more than one element exists.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        T GetSingle(Func<T, bool> exp);

        /// <summary>
        /// Tells database context to track entity for later insertion.
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Saves changes made to database context to database.
        /// </summary>
        /// <returns>Number of state entries for entities/relationships
        /// written to database (used for many-to-many relationships).</returns>
        int Commit();
    }
}
