using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(ChipsDbContext db) : base(db)
        { }

        public Device GetById(int id) => GetSingle(d => d.Id == id);

        public IEnumerable<Device> GetAllByCustId(int customerId)
        {
            return _db.Devices.Where(x => x.CustomerId == customerId);
        }

        public IEnumerable<Device> Search(string searchValue)
        {
            var result = new List<Device>();
            if (!String.IsNullOrEmpty(searchValue))
            {
                result.AddRange(_db.Devices.Where(c => c.Make.Contains(searchValue)));
                result.AddRange(_db.Devices.Where(c => c.ModelNumber.Contains(searchValue)));
                result.AddRange(_db.Devices.Where(c => c.OperatingSystem.Contains(searchValue)));
                result.AddRange(_db.Devices.Where(c => c.Password.Contains(searchValue)));                
            }
            return result;
        }
    }
}
