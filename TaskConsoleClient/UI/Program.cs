using Ninject;
using Ninject.Modules;
using TaskConsoleClient.Manager;
﻿using System;

namespace TaskConsoleClient.UI
{
    public static class Program
    {
        public static void Main()
        {
            var module = new TaskManagerModule();
            var kernel = new StandardKernel(module);
            var consoleHelper = kernel.Get<ConsoleHelper>();
            string s;

            while (( s = Console.ReadLine()) != null)
                consoleHelper.ExecuteCommand(s);

        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandManager>().To<CommandManager>();
            Bind<IConnection>().To<NetTcpConnection>();
        }
    }
}

