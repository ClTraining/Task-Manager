using System;
using System.ServiceModel;

namespace TaskManagerHost.WCFServer
{
    static class TaskManagerApplication
    {
        private static void Main()
        {
            const string address = "net.tcp://localhost:44444";
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