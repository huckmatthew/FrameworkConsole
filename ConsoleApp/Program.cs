using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Demo.ApplicationInsights;
using Demo.ApplicationInsights.Configure;
using Demo.ApplicationInsights.Interface;

namespace ConsoleApp
{
    class Program
    {
        public static IContainer ApplicationContainer { get; private set; }
        static void Main(string[] args)
        {
            ConfigureAutoFac();
            ConfigureMyApplicationInsights();

            var logger = ApplicationInsightsAppLogger.GetLogger();

            logger.LogDebug("Application Started", "Startup", new { badthing = true }, null);

            Console.WriteLine("Done!");
            Console.ReadLine();
            logger.LogDebug("Application Stopped", "Stopped", null, null);
            logger.Flush();
        }

        private static void ConfigureMyApplicationInsights()
        {
            var configsettings = new ApplicationInsightsConfig();
            configsettings.ApplicationName = "DemoApp1";
            configsettings.Enabled = bool.Parse(ConfigurationManager.AppSettings["Enabled"]);
            configsettings.LoggingLevel = (LogLevel) Enum.Parse(typeof(LogLevel),  ConfigurationManager.AppSettings["LogLevel"]);
            configsettings.InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];
            configsettings.Environment = ConfigurationManager.AppSettings["Environment"];

            ConfigureApplicationInsights.Config(configsettings);
        }

        private static void ConfigureAutoFac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(ApplicationInsightsAppLogger.GetLogger()).As<IAppLogger>();
            ApplicationContainer = builder.Build();
        }
    }
}
