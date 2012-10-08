using System.ServiceModel;
using ConnectToWcf;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using System;
using TaskManagerConsole.ConcreteHandlers;
using TaskManagerService.WCFService;

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
                kernel.Get<ConsoleHelper>().Execute(s);
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
            this.Bind(x => x.FromThisAssembly()
                               .SelectAllClasses()
                               .InNamespaceOf<BaseHandler>()
                               .BindBase()
                               .Configure(b => b.InThreadScope()));

            Bind<IClientConnection>().To<ClientConnection>();
        }
    }
}

