﻿using System;
using System.ServiceModel;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            using (var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444"))
            {
                var client = factory.CreateChannel();
                while (true)
                {
                    var task = new ConsoleHelper().Parse(Console.ReadLine());
                    var res = client.AddTask(task);
                    Console.WriteLine(res.Id);
                }
            }
        }
    }
}
