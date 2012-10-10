using System;
using ConnectToWcf;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using TaskManagerClientLibrary.ConcreteHandlers;

namespace TaskManagerClientLibrary
{
    public class Application
    {
        public void Run()
        {
            Console.Title = "Task Manager Client";

            var module = new TaskManagerModule();

            var kernel = new StandardKernel(module);

            for (string s; ((s = Console.ReadLine()) != null);)

                kernel.Get<LineParser>().ExecuteCommand(s);
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
            Bind<ArgumentConverter<string>>().To<ArgumentConverter<string>>();
            Bind<ArgumentConverter<int>>().To<ArgumentConverter<int>>();
            Bind<IClientConnection>().To<ClientConnection>();
        }
    }
}
