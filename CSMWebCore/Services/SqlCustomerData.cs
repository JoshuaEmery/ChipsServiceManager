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

        public Customer GetById(int id)
        {
            return _db.Find<Customer>(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _db.Customers;
        }

        public IEnumerable<Customer> Search(string searchValue)
        {
            var result = new List<Customer>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(_db.Customers.Where(c => c.FirstName.Contains(searchValue)));
                result.AddRange(_db.Customers.Where(c => c.LastName.Contains(searchValue)));
                result.AddRange(_db.Customers.Where(c => c.Phone.Contains(searchValue)));
                result.AddRange(_db.Customers.Where(c => c.StudentId.Contains(searchValue)));
                result.AddRange(_db.Customers.Where(c => c.Email.Contains(searchValue)));
            }
            return result;
        }
    }
}
