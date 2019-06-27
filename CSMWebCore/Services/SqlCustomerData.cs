using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class SqlCustomerData : ICustomerData
    {
        private ChipsDbContext _db;
        public SqlCustomerData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(Customer customer)
        {
            _db.Add(customer);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public Customer Get(int id)
        {
            return _db.Find<Customer>(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _db.Customers;
        }
    }
}
