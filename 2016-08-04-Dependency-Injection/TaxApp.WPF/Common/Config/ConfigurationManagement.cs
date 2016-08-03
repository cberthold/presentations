using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using TaxApp.WPF.Common.Config.Interfaces;

namespace TaxApp.WPF.Common.Config
{
    public class ConfigurationManagement : IConfigurationManagement
    {
        public const string KEY_DEFAULT_ENVIRONMENT = "DefaultEnvironemnt";
        public EnvironmentType CurrentEnvironment { get; set; }

        public ConfigurationManagement()
        {
            CurrentEnvironment = EnvironmentType.Local;

            var defaultEnvironment = ConfigurationManager.AppSettings[KEY_DEFAULT_ENVIRONMENT];

            if (defaultEnvironment == null) return;

            EnvironmentType defaultType;

            if (Enum.TryParse(defaultEnvironment, true, out defaultType))
            {
                CurrentEnvironment = defaultType;
            }


        }

        private static IDictionary<EnvironmentType, EnvironmentConfiguration> configurationMapping;

        static ConfigurationManagement()
        {
            var configurations = EnvironmentConfiguration.GetConfigurations();
            configurationMapping = configurations.ToDictionary(a => a.EnvironmentType, a => a);
        }

        public List<EnvironmentConfiguration> GetAvailableConfigurations()
        {
            return configurationMapping.Values.ToList();
        }

        private ConfigurationWrapper LoadConfigurationInternal(EnvironmentType environmentType)
        {
            var environment = configurationMapping[environmentType];
            var configFile = environment.ConfigFile;
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;

            var config = ConfigurationManager.OpenMappedExeConfiguration(
                    configFileMap, ConfigurationUserLevel.None);

            var wrapper = new ConfigurationWrapper(config);

            return wrapper;
        }


        public ConfigurationWrapper GetConfiguration()
        {
            return LoadConfigurationInternal(CurrentEnvironment);
        }

        public class ConfigurationWrapper
        {
            private Configuration _config;

            internal ConfigurationWrapper(Configuration config)
            {
                _config = config;
            }

            public KeyValueConfigurationCollection AppSettings
            {
                get
                {
                    AppSettingsSection section = _config.GetSection("appSettings") as AppSettingsSection;

                    if(section == null)
                    {
                        return new KeyValueConfigurationCollection();
                    }

                    return section.Settings;
                }
            }

            public ConnectionStringSettingsCollection ConnectionStrings
            {
                get
                {
                    var section = _config.GetSection("connectionStrings") as ConnectionStringsSection;
                    if (section == null)
                    {
                        return new ConnectionStringSettingsCollection();
                    }
                    return section.ConnectionStrings;
                }
            }
        }
    }
}
