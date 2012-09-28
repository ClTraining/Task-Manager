using System;
using System.ServiceModel;

namespace TaskManagerHost.WCFServer
{
    static class TaskManagerApplication
    {
        private static void Main()
        {
            using (var serviceHost = new ServiceHost(typeof(TaskManagerService)))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press Enter to terminate the host...");
                Console.ReadLine();
            }
        }
    }
}