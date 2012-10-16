using System.Configuration;

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
