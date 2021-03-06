﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Consul.Test
{
    public class UtilTest
    {
        [Fact]
        public void GoDurationParsing()
        {
            Assert.Equal("150ms", new TimeSpan(0, 0, 0, 0, 150).ToGoDuration());
            Assert.Equal("26h3m4.005s", new TimeSpan(1, 2, 3, 4, 5).ToGoDuration());
            Assert.Equal("2h3m4.005s", new TimeSpan(0, 2, 3, 4, 5).ToGoDuration());
            Assert.Equal("3m4.005s", new TimeSpan(0, 0, 3, 4, 5).ToGoDuration());
            Assert.Equal("4.005s", new TimeSpan(0, 0, 0, 4, 5).ToGoDuration());
            Assert.Equal("5ms", new TimeSpan(0, 0, 0, 0, 5).ToGoDuration());
            Assert.Equal("1ms", TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 1000 / 2).ToGoDuration());
            Assert.Equal("0ms", TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 1000 / 3).ToGoDuration());
        }
    }
}