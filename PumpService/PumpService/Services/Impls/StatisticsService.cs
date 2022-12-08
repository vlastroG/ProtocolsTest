using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumpService
{
    public class StatisticsService : IStatisticsService
    {
        public int SuccessCount { get; set; }

        public int ErrorCount { get; set; }

        public int AllCount { get; set; }
    }
}