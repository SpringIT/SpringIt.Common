using System;
using System.ComponentModel;
using System.Configuration;

namespace SpringIt.Config
{
    public class ConfigReader : IConfigReader
    {
        public bool HasValue(string key)
        {
            return (ConfigurationManager.AppSettings[key] != null);
        }

        public T GetValue<T>(string key)
        {
            if (!HasValue(key))
            {
                throw new Exception(key + " not found in configuration");
            }

            var value = ConfigurationManager.AppSettings[key];
            return CastTo<T>(value);
        }

        private static T CastTo<T>(string value)
        {
            var convertor = TypeDescriptor.GetConverter(typeof(T));
            return (T)convertor.ConvertFrom(value);
        }
    }
}
