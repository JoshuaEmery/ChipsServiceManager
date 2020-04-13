using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{/// <summary>
/// Class which interacts with the database for Customer data
/// </summary>
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ChipsDbContext db) : base(db)
        { }

        public IEnumerable<Customer> Search(string searchValue)
        {
            var result = new List<Customer>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(context.Customers.Where(c => c.FirstName.Contains(searchValue)));
                result.AddRange(context.Customers.Where(c => c.LastName.Contains(searchValue)));
                result.AddRange(context.Customers.Where(c => c.Phone.Contains(searchValue)));
                result.AddRange(context.Customers.Where(c => c.StudentId.Contains(searchValue)));
                result.AddRange(context.Customers.Where(c => c.Email.Contains(searchValue)));
            }
            return result;
        }
    }
}
