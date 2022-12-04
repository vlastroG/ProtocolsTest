﻿using PumpClient.PumpServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PumpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            PumpServiceClient client = new PumpServiceClient(instanceContext);

            client.UpdateAndCompileScript("C:\\Users\\vlast\\source\\repos\\ProtocolsTest\\PumpService\\PumpService\\Scripts\\Sample.script");
            client.RunScript();

            Console.ReadKey(true);
            client.Close();
        }
    }
}
