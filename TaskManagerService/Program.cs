using System;
using System.ServiceModel;
using CommandQueryLibrary.ServiceSpecifications;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using TaskManagerServiceLibrary;
using TaskManagerServiceLibrary.Commands;
using TaskManagerServiceLibrary.Converters;
using TaskManagerServiceLibrary.Repositories;
using TaskManagerServiceLibrary.TaskMapper;
using TaskManagerServiceLibrary.ToDoList;

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
            Bind<IRepository>().To<JsonStorage>().InSingletonScope();
            Bind<ITaskMapper>().To<TaskMapper>();
            Bind<FileOperationsWrapper>().ToSelf().WithConstructorArgument("path", "save.txt");
            Bind<ITodoList>().To<ToDoList>();
            Bind<IArgToCommandConverter>().To<ArgToCommandConverter>().WithConstructorArgument("kernel", (c, o) => c.Kernel);

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
                               .Configure(s => s.WithConstructorArgument("todoList", (c, o) => c.Kernel.Get<ITodoList>()))
                );
        }
    }
}