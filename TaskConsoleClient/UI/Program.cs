﻿using Ninject;
using Ninject.Modules;
using TaskConsoleClient.ConcreteHandlers;
using TaskConsoleClient.Manager;
using System;

namespace TaskConsoleClient.UI
{
    public static class Program
    {
        public static void Main()
        {
            Console.Title = "Task Manager Client";
            Console.WriteLine(TestConnection()
                ? "Connection established."
                : "Wrong server address.");

            var module = new TaskManagerModule();

            var kernel = new StandardKernel(module);

            for (string s; ((s = Console.ReadLine()) != null); )
                kernel.Get<ConsoleHelper>().Execute(s);
        }

        private static bool TestConnection()
        {
            return new NetTcpConnection().TestConnection();
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandHandler>().To<ConcreteHandlerAddTask>();
            Bind<ICommandHandler>().To<ConcreteHandlerMarkCompleted>();
            Bind<ICommandHandler>().To<ConcreteHandlerShowAll>();
            Bind<ICommandHandler>().To<ConcreteHandlerShowSingleTask>();
            Bind<ICommandManager>().To<CommandManager>();
            Bind<IConnection>().To<NetTcpConnection>();
        }
    }
}

