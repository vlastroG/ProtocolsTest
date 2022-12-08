using PumpClient.PumpServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumpClient
{
    internal class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(StatisticsService statistics)
        {
            Console.Clear();
            Console.WriteLine("Update script run statistic");
            Console.WriteLine($"All\t\tcounts: {statistics.AllCount}");
            Console.WriteLine($"Successful\tcounts: {statistics.SuccessCount}");
            Console.WriteLine($"Error\t\tcounts: {statistics.ErrorCount}");
        }
    }
}
