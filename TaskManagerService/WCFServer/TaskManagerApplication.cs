using System;
using System.ServiceModel;


namespace TaskManagerHost.WCFServer
{
    internal static class TaskManagerApplication
    {
        private const string Address = "net.tcp://localhost:44444";

        private static void Main()
        {
            const string address = "net.tcp://localhost:44444";
            using (var serviceHost = new ServiceHost(typeof (TaskManagerService), new Uri(address)))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press Enter to terminate the host...");
                Console.ReadLine();
            }
        }
    }
}