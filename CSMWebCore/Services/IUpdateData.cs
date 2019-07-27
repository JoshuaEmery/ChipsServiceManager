using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IUpdateData
    {
        Update Get(Guid id);
        int GetTicketId(Guid id);
        void Add(Update update);
        int Commit();
    }
}
