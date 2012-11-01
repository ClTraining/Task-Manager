using EntitiesLibrary;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class CompleteServiceCommand : IServiceCommand
    {
        public int Id { get; set; }
        
        public ServiceTask ExecuteCommand(ServiceTask task)
        {
            task.IsCompleted = true;
            return task;
        }
    }

    public class CompleteServiceCommandTests
    {
        [Fact]
        public void command_should_complete_task()
        {
            var repo = Substitute.For<IRepository>();
            var serviceTask = new ServiceTask { Id = 1 };
            repo.Select(1).Returns(serviceTask);

            var command = new CompleteServiceCommand();
        }
    }
}