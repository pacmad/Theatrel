﻿using System;
using System.Collections.Generic;
using System.Linq;
using theatrel.Interfaces.Playbill;

namespace theatrel.Lib.Tickets
{
    internal class PerformanceTickets : IPerformanceTickets
    {
        public string Description { get; set; }

        public DateTime LastUpdate { get; set; }

        public IDictionary<string, IDictionary<int, int>> Tickets { get; set; }
            = new Dictionary<string, IDictionary<int, int>>();

        public int GetMinPrice()
        {
            return !Tickets.Any()
                ? 0
                : Tickets.Min(block => block.Value.Keys.Any() ? block.Value.Keys.Min() : 0);
        }
    }
}
