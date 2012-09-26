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
        private readonly IToDoList _tasks;

        public TaskManagerService()
        {
            this._tasks = new ToDoList(new TaskFactory(), new MemoRepository());
            Console.WriteLine("added new task");
        }

        public TaskManagerService(IToDoList tasks)
        {
            this._tasks = tasks;
        }

        public ITask AddTask(ContractTask task)
        {
            return _tasks.AddTask(task);
        }
    }


    public class TaskManagerServiceTests
    {
        private readonly ITask _incomingTask = new ContractTask();
        private readonly ITask _outgoingTask = new ContractTask();
        private readonly IToDoList _list = Substitute.For<IToDoList>();
        private readonly ITaskManagerService _manager;

        public TaskManagerServiceTests()
        {
            _manager = new TaskManagerService(_list);         
        }

        [Fact]
        public void should_send_and_return_task()
        {
            //arrange
            _list.AddTask(_outgoingTask).Returns(_incomingTask);

            var task = _manager.AddTask(_outgoingTask as ContractTask);

            task.Should().Be(_incomingTask);
        }
    }
}