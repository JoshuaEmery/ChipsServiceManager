using CSMWebCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface XIUpdateData
    {
        XUpdate Get(Guid id);
        int GetTicketId(Guid id);
        void Add(XUpdate update);
        int Commit();
    }
}
