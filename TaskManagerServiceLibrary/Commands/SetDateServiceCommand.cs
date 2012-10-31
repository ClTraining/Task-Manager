using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.Commands
{
    public class SetDateServiceCommand : IServiceCommand
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }

        public void ExecuteCommand(IRepository repo)
        {
            var task = repo.Select(Id);
            if(task.IsCompleted)
                throw new Exception();
            task.DueDate = DueDate;
        }
    }

    public class SetDateServiceCommandTest
    {
        private SetDateTaskArgs args = new SetDateTaskArgs {Id = 1, DueDate = DateTime.Today};
        private readonly IRepository repo = Substitute.For<IRepository>();
        readonly SetDateServiceCommand command = new SetDateServiceCommand();
        
        [Fact]
        public void command_should_set_date_for_task()
        {
            var serviceTask = new ServiceTask {Id = 1, DueDate = default(DateTime)};
            repo.Select(1).Returns(serviceTask);

            command.ExecuteCommand(repo);
            serviceTask.DueDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void should_throw_exception_if_task_is_completed()
        {
            var serviceTask = new ServiceTask { Id = 1, DueDate = default(DateTime), IsCompleted = true};
            repo.Select(1).Returns(serviceTask);

            Action action = () => command.ExecuteCommand(repo);

            action.ShouldThrow<Exception>();
        }
    }
}