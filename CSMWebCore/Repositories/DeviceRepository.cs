using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        private ICustomerRepository _customers { get; set; }

        public DeviceRepository(ChipsDbContext db, ICustomerRepository customerRepository) : base(db)
        {
            _customers = customerRepository;
        }

        public IEnumerable<Device> GetAllByCustId(int customerId)
        {
            return context.Devices.Where(x => x.Customer.Id == customerId);
        }

        public IEnumerable<Device> Search(string searchValue)
        {
            var result = new List<Device>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(context.Devices.Where(c => c.Make.Contains(searchValue)));
                result.AddRange(context.Devices.Where(c => c.ModelNumber.Contains(searchValue)));
                result.AddRange(context.Devices.Where(c => c.OperatingSystem.Contains(searchValue)));
                result.AddRange(context.Devices.Where(c => c.Password.Contains(searchValue)));                
            }
            return result;
        }
    }
}
