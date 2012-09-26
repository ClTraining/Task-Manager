﻿using System;
using System.ServiceModel;
using EntitiesLibrary;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var task = new ContractTask();
            var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");
            var client = factory.CreateChannel();
            client.AddTask(task);
        }
    }
}
