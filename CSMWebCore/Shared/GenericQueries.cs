using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class GenericQueries
    {
        public static IEnumerable<TEntity> Get<TEntity>(
           this DbSet<TEntity> dbSet,
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "") where TEntity : class
        {
            IQueryable<TEntity> query = dbSet;

            // filter query given a function delegate or lambda expression
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // call include to bring related entities into the query (for example,
            // if calling from the customer repository and needing related devices)
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // order by a query given a function delegate or lambda expression
            // that takes an IQueryable and returns an IOrderedQueryable
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}
