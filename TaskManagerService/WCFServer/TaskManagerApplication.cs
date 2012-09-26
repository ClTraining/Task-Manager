using System;
using System.ServiceModel;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerService.TaskManager;
using Xunit;

namespace TaskManagerService.WCFServer
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
        private readonly IToDoList tasks;
        private ITask task;

        public TaskManagerService() { }

        public TaskManagerService(IToDoList tasks)
        {
            this.tasks = tasks;
        }

        public ITask AddTask(ITask task)
        {
            return tasks.AddTask(task);
        }
    }


    public class TaskManagerServiceTests
    {
        private readonly ITask incomingTask = Substitute.For<ITask>();
        private readonly ITask outgoingTask = Substitute.For<ITask>();
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

            var task = manager.AddTask(outgoingTask);

            task.Should().Be(incomingTask);
        }
    }
}