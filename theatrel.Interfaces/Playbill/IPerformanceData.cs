﻿using System;
using theatrel.Interfaces.Autofac;

namespace theatrel.Interfaces.Playbill
{
    public interface IPerformanceData : IDIRegistrable
    {
        string Location { get; set; }
        string Name { get; set; }
        DateTime DateTime { get; set; }
        string Url { get; set; }
        string Type { get; set; }

        int MinPrice { get; set; }
    }
}
