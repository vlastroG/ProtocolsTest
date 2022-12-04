using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumpService
{
    public interface IStatisticsService
    {
        int SuccessCount { get; set; }

        int ErrorCount { get; set; }

        int AllCount { get; set; }
    }
}
