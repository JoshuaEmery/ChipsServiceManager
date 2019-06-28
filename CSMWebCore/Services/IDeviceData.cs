using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IDeviceData
    {
        List<Device> GetAll();
        Device Get(int id);
        void Add(Device device);
        int Commit();
    }
}
