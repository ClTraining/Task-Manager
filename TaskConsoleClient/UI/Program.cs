<<<<<<< HEAD
﻿using TaskConsoleClient.Manager;
﻿using System;
<<<<<<< HEAD
=======
using System.ServiceModel;
using EntitiesLibrary;
using TaskConsoleClient.Manager;
using TaskManagerHost.WCFServer;
>>>>>>> updated


namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var client = new CommandManager();
            while (true)
            {
                client.Run();
            }
        }
    }
}
=======
﻿namespace TaskConsoleClient.UI
{
    {
        static void Main()
        {
<<<<<<< HEAD
            var task = new ContractTask();
            var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");
            var client = factory.CreateChannel();
            client.AddTask(task);
        }
    }
}
>>>>>>> added wcf client
=======
            //var task = new ConsoleHelper().Parse(Console.ReadLine())
            //var factory = new ChannelFactory<ITaskManagerService>(new NetTcpBinding(), "net.tcp://localhost:44444");
            //var client = factory.CreateChannel();
            //client.AddTask(task);
        }
    }
}
>>>>>>> updated
