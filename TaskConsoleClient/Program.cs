using System.ServiceModel;
using ConnectToWcf;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using System;
using TaskManagerClientLibrary;
using TaskManagerClientLibrary.ConcreteHandlers;
using TaskManagerServiceLibrary;

namespace TaskManagerConsole
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

                kernel.Get<LineParser>().ExecuteCommand(s);
        }

        private static bool TestConnection()
        {
            return new ChannelFactory<ITaskManagerService>("tcpEndPoint")
                .CreateChannel()
                .TestConnection();
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromAssemblyContaining<ICommandHandler>().SelectAllClasses()
                               .InNamespaceOf<ICommandHandler>()
                               .BindAllInterfaces()
                               );

            Bind<IClientConnection>().To<ClientConnection>();
        }
    }
}

