using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IEnumerable<Customer> Search(string searchValue);
    }
}
