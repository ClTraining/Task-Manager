<<<<<<< HEAD
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

=======
﻿using System;
using Ninject;
using Ninject.Modules;
using TaskConsoleClient.Manager;
>>>>>>> 4dd826778d6c0aa0bb31c5c3e59682e9053b13d9

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
<<<<<<< HEAD
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
=======
            var module = new TaskManagerModule();
            var kernel = new StandardKernel(module);
            var consoleHelper = kernel.Get<IConsoleHelper>();
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                consoleHelper.Parse(s);
            }
        }
    }
    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConsoleHelper>().To<ConsoleHelper>();
            Bind<ICommandManager>().To<CommandManager>();
>>>>>>> 4dd826778d6c0aa0bb31c5c3e59682e9053b13d9
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
