using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Demo.ApplicationInsigts;
using Demo.ApplicationInsigts.Configure;
using Demo.ApplicationInsigts.Interface;

namespace ConsoleApp452
{
    class Program
    {
        public static IContainer ApplicationContainer { get; private set; }
        static void Main(string[] args)
        {
            ConfigureAutoFac();
            ConfigureApplicationInsights();

            var logger = ApplicationInsightsAppLogger.GetLogger();

            logger.LogDebug("Application Started", "", null, null);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static void ConfigureApplicationInsights()
        {
            var configsettings = new ApplicationInsightsConfig();
            configsettings.ApplicationName = "DemoApp1";
            configsettings.Enabled = bool.Parse(ConfigurationManager.AppSettings["Enabled"]);
            configsettings.InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];
            configsettings.Environment = ConfigurationManager.AppSettings["Environment"];

            Demo.ApplicationInsigts.Configure.ConfigureApplicationInsights.Config(configsettings);
        }

        private static void ConfigureAutoFac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(ApplicationInsightsAppLogger.GetLogger()).As<IAppLogger>();
            ApplicationContainer = builder.Build();
        }
    }
}
