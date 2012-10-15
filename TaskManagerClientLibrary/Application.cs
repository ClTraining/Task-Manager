using System;
using System.Configuration;
using ConnectToWcf;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using TaskManagerClientLibrary.ConcreteHandlers;
using System.Linq;

namespace TaskManagerClientLibrary
{
    public class Application
    {
        public void Run()
        {
            Console.Title = "Task Manager Client";

            var module = new TaskManagerModule();
            var kernel = new StandardKernel(module);

            var notifier = kernel.Get<UserNotifier>();

            var greeting = notifier.GenerateGreeting();
            Console.WriteLine(greeting);

            for (string s; ((s = Console.ReadLine()) != null); )
            {
                kernel.Get<LineParser>().ExecuteCommand(s);
            }

        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromAssemblyContaining<ICommand>().SelectAllClasses()
                               .InNamespaceOf<ICommand>()
                               .BindAllInterfaces()
                               );

            Bind<ArgumentConverter<object>>().ToSelf();

            Bind<ConfigurationManager>().ToSelf();
            Bind<ExitManager>().ToSelf();

            var configManager = new ConfigurationManager();
            var address = configManager.GetAddress();

            Bind<UserNotifier>().ToSelf().WithConstructorArgument("address", address);
            Bind<IClientConnection>().To<ClientConnection>().WithConstructorArgument("address", address);
        }
    }
}
