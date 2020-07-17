﻿using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using theatrel.Interfaces;

namespace theatrel.TLBot.Tests
{
    internal class PlayBillResolverMock
    {
        public DateTime StartDate;
        public IPerformanceFilter Filter;

        private readonly Mock<IPlayBillDataResolver> _playBillResolverMock = new Mock<IPlayBillDataResolver>();
        public IPlayBillDataResolver Object => _playBillResolverMock.Object;

        public PlayBillResolverMock()
        {
            _playBillResolverMock.Setup(h => h.RequestProcess(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<IPerformanceFilter>(), It.IsAny<CancellationToken>()))
                    .Callback<DateTime, DateTime, IPerformanceFilter, CancellationToken>((dtStart, stEnd, filterResult, cToken) =>
                    {
                        StartDate = dtStart;
                        Filter = filterResult;
                    }).Returns(() => Task.FromResult(new IPerformanceData[0]));
        }
    }
}
