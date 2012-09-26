using System;
using System.ServiceModel;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using Ninject;
using Ninject.Modules;
using TaskManagerHost.DataBaseAccessLayer;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerHost.WCFServer
{
    static class TaskManagerApplication
    {
        const string Address = "net.tcp://localhost:44444";

        static void Main()
        {
            using (var serviceHost = new ServiceHost(typeof(TaskManagerService), new Uri(Address)))
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
            Bind<IRepository>().To<MemoRepository>();
            Bind<ITaskFactory>().To<TaskFactory>();
            Bind<IToDoList>().To<ToDoList>();
            Bind<ITaskManagerService>().To<TaskManagerService>();
        }
    }

    public class TaskManagerService : ITaskManagerService
    {
        readonly TaskManagerModule module;
        private IKernel kernel;

        public TaskManagerService()
        {
            module = new TaskManagerModule();
            kernel = new StandardKernel(module);
            Console.WriteLine("added new task");
        }

        public ServiceTask AddTask(ContractTask task)
        {
            return kernel.Get<ToDoList>().AddTask(task);
        }
    }


    public class TaskManagerServiceTests
    {
        private readonly ServiceTask incomingTask = new ServiceTask();
        private readonly ContractTask outgoingTask = new ContractTask();
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService manager;

        public TaskManagerServiceTests()
        {
            manager = new TaskManagerService();         
        }

        [Fact]
        public void should_send_and_return_task()
        {
            //arrange
            list.AddTask(outgoingTask).Returns(incomingTask);

            var task = manager.AddTask(outgoingTask);

            task.Should().Be(incomingTask);
        }
    }
}