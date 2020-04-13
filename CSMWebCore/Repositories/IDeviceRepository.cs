using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        IEnumerable<Device> GetAllByCustId(int customerId);
        IEnumerable<Device> Search(string searchValue);
    }
}
