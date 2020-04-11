using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetById(int id);
        IEnumerable<Customer> Search(string searchValue);
    }
}
