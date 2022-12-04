using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace PumpService
{
    public class ScriptService : IScriptService
    {
        private CompilerResults _results = null;

        private readonly IStatisticsService _statisticsService;

        private readonly ISettingsService _settingsService;

        private readonly IPumpServiceCallback _pumpServiceCallback;

        public ScriptService(
            IStatisticsService statisticsService,
            ISettingsService settingsService,
            IPumpServiceCallback pumpServiceCallback)
        {
            _statisticsService = statisticsService;
            _settingsService = settingsService;
            _pumpServiceCallback = pumpServiceCallback;
        }


        public bool Compile()
        {
            try
            {
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;
                compilerParameters.ReferencedAssemblies.AddRange(
                    new string[]
                    {
                    "System.dll",
                    "System.Core.dll",
                    "System.Data.dll",
                    "Microsoft.CSharp.dll"
                    });
                compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                byte[] buffer;
                using (FileStream fileStream = new FileStream(_settingsService.FileName, FileMode.Open))
                {
                    int length = (int)fileStream.Length;
                    buffer = new byte[length];
                    int count;
                    int sum = 0;
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;
                    }
                }
                CSharpCodeProvider provider = new CSharpCodeProvider();
                _results = provider.CompileAssemblyFromSource(compilerParameters, System.Text.Encoding.UTF8.GetString(buffer));
                if (_results.Errors != null && _results.Errors.Count > 0)
                {
                    string compileErrors = string.Join(Environment.NewLine, _results.Errors);
                    _results = null;
                    Console.WriteLine(compileErrors);
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                _results = null;
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void Run(int count)
        {
            if (_results != null)
            {
                if (!Compile())
                {
                    return;
                }
            }

            Type type = _results.CompiledAssembly.GetType("Sample.SampleScript");
            if (type == null)
            {
                return;
            }
            MethodInfo entryPointMethod = type.GetMethod("EntryPoint");
            if (entryPointMethod == null)
            {
                return;
            }

            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    if ((bool)entryPointMethod.Invoke(Activator.CreateInstance(type), null))
                    {
                        _statisticsService.SuccessCount++;
                    }
                    else
                    {
                        _statisticsService.ErrorCount++;
                    }
                    _statisticsService.AllCount++;
                    _pumpServiceCallback.UpdateStatistics((StatisticsService)_statisticsService);
                }
            });

        }
    }
}