using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxApp.WPF.Common.Config
{
    public enum EnvironmentType
    {
        Local
    }
    

    public class EnvironmentConfiguration
    {
        public EnvironmentType EnvironmentType { get; private set; }
        public string EnvironmentName { get; private set; }
        public string ConfigFile { get; private set; }

        public static List<EnvironmentConfiguration> GetConfigurations()
        {
            var list = new List<EnvironmentConfiguration>()
            {
                new EnvironmentConfiguration
                {
                    EnvironmentType = EnvironmentType.Local,
                    EnvironmentName = "Local",
                    ConfigFile = "App.local.Config",
                },
                
            };

            return list;
        }
    }
}
