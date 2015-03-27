using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.Configuration
{
    public class DefaultConfigurationReader : IConfigurationReader
    {
        public object GetSection(string sectionName)
        {
            return System.Configuration.ConfigurationManager.GetSection(sectionName);
        }
        
        public T GetSection<T>(string sectionName) where T : class, new()
        {
            return GetSection(sectionName) as T;
        }
    }
}
