using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PumpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PumpService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PumpService.svc or PumpService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _scriptService;

        private readonly IStatisticsService _statisticsService;

        private readonly ISettingsService _settingsService;

        private IPumpServiceCallback _CallBack
        {
            get
            {
                if (OperationContext.Current != null)
                    return OperationContext.Current.GetCallbackChannel<IPumpServiceCallback>();
                else
                    return null;
            }
        }

        public PumpService()
        {
            _statisticsService = new StatisticsService();
            _settingsService = new SettingsService();
            _scriptService = new ScriptService(_statisticsService, _settingsService, _CallBack);
        }


        public void RunScript()
        {
            _scriptService.Run(10);
        }

        public void UpdateAndCompileScript(string fileName)
        {
            _settingsService.FileName = fileName;
            _scriptService.Compile();
        }
    }
}
