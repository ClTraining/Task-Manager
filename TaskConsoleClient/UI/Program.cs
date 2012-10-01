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
            var module = new TaskManagerModule(TestConnection());

            var kernel = new StandardKernel(module);

            for (string s; ((s = Console.ReadLine()) != null); )
                kernel.Get<ConsoleHelper>().ExecuteCommand(s);
        }

        private static string TestConnection()
        {
            var address = "";
            var res = false;
            while (res != true)
            {
                Console.Clear();
                Console.Write("Enter server address: ");
                address = Console.ReadLine();
                res = new NetTcpConnection(address).TestConnection();
            }
            Console.WriteLine("Connection established\nEnter your command:");
            return address;
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        private readonly string address;
        public TaskManagerModule(string address)
        {
            this.address = address;
        }

        public override void Load()
        {
            Bind<ICommandManager>().To<CommandManager>();
            Bind<IConnection>()
                .To<NetTcpConnection>()
                .WithConstructorArgument("address", address);
            new NetTcpConnection(address).TestConnection();
        }
    }
}

