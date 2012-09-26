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
            var module = new TaskManagerModule();

            IKernel result = new StandardKernel(module);

            //using (var serviceHost = new ServiceHost(result.Get<ITaskManagerService>(), new Uri(Address)))
            using (var serviceHost = new ServiceHost(typeof(TaskManagerService), new Uri(Address)))
            {
                serviceHost.CloseTimeout = new TimeSpan(1, 0, 0, 0);
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
            Bind<IRepository>().To<MemoRepository>().InSingletonScope();
            Bind<ITaskFactory>().To<TaskFactory>().InSingletonScope();
            Bind<IToDoList>().To<ToDoList>().InSingletonScope();
            Bind<ITaskManagerService>().To<TaskManagerService>().InSingletonScope();
        }
    }

    public class TaskManagerService : ITaskManagerService
    {
        private readonly IToDoList tasks;

        public TaskManagerService()
        {
            this.tasks = new ToDoList(new TaskFactory(), new MemoRepository());
            Console.WriteLine("added new task");
            tasks = new ToDoList(new TaskFactory(), new MemoRepository());
        }

        public TaskManagerService(IToDoList tasks)
        {
            this.tasks = tasks;
        }

        public ServiceTask AddTask(ContractTask task)
        {
            return tasks.AddTask(task);
        }
    }


    public class TaskManagerServiceTests
    {

        private readonly ITask incomingTask = new ContractTask();
        private readonly ITask outgoingTask = new ContractTask();
        private readonly IToDoList list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService manager;

        public TaskManagerServiceTests()
        {
            manager = new TaskManagerService(list);         
        }

        [Fact]
        public void should_send_and_return_task()
        {
            //arrange
            list.AddTask(outgoingTask).Returns(incomingTask);

            var task = manager.AddTask(outgoingTask as ContractTask);

            task.Should().Be(incomingTask);
        }
    }
}