using EntitiesLibrary;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.Commands
{
    public class RenameServiceCommand : IServiceCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ServiceTask ExecuteCommand(ServiceTask task)
        {
            task.Name = Name;
            return task;
        }
    }

    public class RenameServiceCommandTests
    {
        [Fact]
        public void command_should_rename_task()
        {
            var repo = Substitute.For<IRepository>();
            var serviceTask = new ServiceTask { Id = 1, Name = "1"};
            repo.Select(1).Returns(serviceTask);

            var command = new RenameServiceCommand();
            command.ExecuteCommand(serviceTask);

            serviceTask.Name.Should().Be("task1");
        }
    }
}