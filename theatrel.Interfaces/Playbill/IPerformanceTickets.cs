﻿using System;
using System.Collections.Generic;

namespace theatrel.Interfaces.Playbill
{
    public interface IPerformanceTickets
    {
        string Description { get; set; }

        DateTime LastUpdate { get; set; }

        IDictionary<string, IDictionary<int, int>> Tickets { get; }

        int GetMinPrice();
    }
}
