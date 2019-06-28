using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    interface ILogData
    {
        List<Log> GetAll();
        Log Get(int id);
        void Add(Log log);
        int Commit();
    }
}
