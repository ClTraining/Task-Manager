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
            Console.Clear();
            Console.Write("Enter server address: ");
            var address = "";
            var res = "";
            while (res != "Connection established.")
            {
                address = Console.ReadLine();
                res = new NetTcpConnection(address).TestConnection();
                Console.CursorVisible = false;
                Console.WriteLine(res);
                Console.ReadLine();
                Console.Clear();
                TestConnection();
            }

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

