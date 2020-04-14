using CSMWebCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class CustomerQueries
    {
        /// <summary>
        /// Searches relevant Customer fields for a matching search value and 
        /// returns a collection of matching Customers.
        /// </summary>
        public static List<Customer> Search(this DbSet<Customer> dbSet, string searchValue)
        {
            var result = new List<Customer>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                result.AddRange(dbSet.Where(c => c.FirstName.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.LastName.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.Phone.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.StudentId.Contains(searchValue)));
                result.AddRange(dbSet.Where(c => c.Email.Contains(searchValue)));
            }
            return result;
        }
    }
}
