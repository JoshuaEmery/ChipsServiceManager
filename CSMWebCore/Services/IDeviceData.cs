using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IDeviceData
    {
        IEnumerable<Device> GetAll();
        Device GetById(int id);
        void Add(Device device);
        int Commit();
        IEnumerable<Device> GetAllByCustId(int customerId);
        IEnumerable<Device> Search(string searchValue);
    }
}
