
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxApp.WPF.Common.Config.Interfaces;

namespace Flightdocs.Migration.Logic
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfigurationManagement manager, string key)
        {
            if (manager == null) return null;

            var config = manager.GetConfiguration();

            var settings = config.ConnectionStrings[key];

            if (settings == null) return null;

            return settings.ConnectionString;
        }

        public static string GetApplicationSettingKey(this IConfigurationManagement manager, string key)
        {
            if (manager == null) return null;

            var config = manager.GetConfiguration();
            var settings = config.AppSettings;

            if (settings == null) return null;

            var settingsKey = settings[key];

            if (settingsKey == null) return null;

            return settingsKey.Value;
        }

        public static int GetApplicationSettingAsInteger(this IConfigurationManagement manager, string key, int defaultValue = 0)
        {
            string settingValue = manager.GetApplicationSettingKey(key);
            int intValue;

            if (string.IsNullOrWhiteSpace(settingValue) || !int.TryParse(settingValue, out intValue))
            {
                return defaultValue;
            }

            return intValue;
        }

        public static bool GetApplicationSettingAsBoolean(this IConfigurationManagement manager, string key, bool defaultValue = false)
        {
            const string TRUE_LOWER = "true";
            const string FALSE_LOWER = "false";

            string settingValue = manager.GetApplicationSettingKey(key);
            bool boolValue;

            if (string.IsNullOrWhiteSpace(settingValue))
            {
                return defaultValue;
            }
            else if (bool.TryParse(settingValue, out boolValue))
            {
                return boolValue;
            }
            else if (settingValue.ToLower() == TRUE_LOWER)
            {
                return true;
            }
            else if (settingValue.ToLower() == FALSE_LOWER)
            {
                return false;
            }

            throw new InvalidCastException(string.Format("Unable to parse boolean value for application key {0} in environment {1}", key, manager.CurrentEnvironment));
        }

    }
}
