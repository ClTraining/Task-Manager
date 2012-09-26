using System;
using System.ServiceModel;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerHost.TaskManager;
using Xunit;

namespace TaskManagerHost.WCFServer
{
    static class TaskManagerApplication
    {
        static void Main()
        {
            const string address = "net.tcp://localhost:44444";
            using (var serviceHost = new ServiceHost(typeof(TaskManagerService), new Uri(address)))
            {
                serviceHost.Open();
                Console.WriteLine("Host started");
                Console.WriteLine("Press Enter to terminate the host...");
                Console.ReadLine();
            }
        }
    }

    public class TaskManagerService : ITaskManagerService
    {
        private readonly IToDoList _tasks;

        public TaskManagerService() { }

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