using EntitiesLibrary;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class CompleteServiceCommand : IServiceCommand
    {
        public int Id { get; set; }
        
        public void ExecuteCommand(IRepository repo)
        {
            var task = repo.Select(Id);
            task.IsCompleted = true;
            repo.UpdateChanges(task);
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