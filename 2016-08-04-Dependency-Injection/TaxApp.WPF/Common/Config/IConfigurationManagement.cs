using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaxApp.WPF.Common.Config.ConfigurationManagement;

namespace TaxApp.WPF.Common.Config.Interfaces
{
    public interface IConfigurationManagement
    {
        EnvironmentType CurrentEnvironment { get; set; }

        List<EnvironmentConfiguration> GetAvailableConfigurations();
        ConfigurationWrapper GetConfiguration();
    }
}
