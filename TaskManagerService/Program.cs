using System;
using System.ServiceModel;
using CQRS.Commands;
using CQRS.ServiceSpecifications;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using TaskManagerServiceLibrary;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskManager;

namespace TaskManagerService
{
    internal static class TaskManagerApplication
    {
        private static readonly Uri baseAddresses = new Uri("net.tcp://localhost:44444");

        private static void Main()
        {
            Console.Title = "Task Manager Service";
            var kernel = new StandardKernel(new TaskManagerModule());

            using (var serviceHost = new ServiceHost(kernel.Get<ITaskManagerService>(), baseAddresses))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press Enter to terminate the host...");
                Console.ReadLine();
            }
        }
    }

    public class TaskManagerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskManagerService>().To<TaskManagerServiceLibrary.TaskManagerService>();
            Bind<IRepository>().To<MemoRepository>().InSingletonScope();
            Bind<ITaskMapper>().To<TaskMapper>();
            Bind<ITodoList>().To<TodoList>();

            Bind<ISpecificationsConverter>().To<SpecificationsConverter>();

            this.Bind(a => a.FromAssemblyContaining<IServiceSpecification>()
                               .SelectAllClasses()
                               .InNamespaceOf<IServiceSpecification>()
                               .BindAllInterfaces()
                );

            this.Bind(a => a.FromAssemblyContaining<IServiceCommand>()
                               .SelectAllClasses()
                               .InNamespaceOf<IServiceCommand>()
                               .BindAllInterfaces()
                );
        }
    }
}