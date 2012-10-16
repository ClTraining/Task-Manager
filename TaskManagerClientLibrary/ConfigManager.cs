using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClientLibrary
{
    public class ConfigurationManager
    {
        public string GetAddress()
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var address = config.AppSettings.Settings["connectionAddress"].Value;
            return address;
        }
    }
}
