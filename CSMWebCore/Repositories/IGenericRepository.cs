using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets a collection of entity objects, with filtering and ordering by lambda expressions.
        /// includeProperties is a comma-separated string for specifying related entities, which
        /// allows the query to return entity properties related to the top-level entity.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Gets a single entity object with the corresponding ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(int id);

        /// <summary>
        /// Adds the given entity to the context for later database insertion.
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Deletes the entity from the context, given its ID.
        /// Changes are not yet written to the database.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Deletes the entity from the context. Changes are not yet 
        /// written to the database.
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Updates a given entity in the context and marks it as modified.
        /// Changes are not yet written to the database.
        /// </summary>
        /// <param name="entityToUpdate"></param>
        void Update(TEntity entityToUpdate);


        // temp
        int Commit();
    }
}
