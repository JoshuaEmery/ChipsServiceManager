﻿using CSMWebCore.Entities;
using CSMWebCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public interface IServicePriceData
    {
        decimal GetTotalPrice(IEnumerable<LogType> logs);
        decimal GetPriceOfServiceType(LogType service);
    }
}
