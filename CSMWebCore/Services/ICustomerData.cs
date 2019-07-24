using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    /// <summary>
    /// Interface for accessing customer data.  By using an interface this allows dependency injection to be used.
    /// </summary>
    public interface ICustomerData
    {
        
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
        void Add(Customer customer);
        int Commit();
        IEnumerable<Customer> Search(string searchValue);
    }

}
