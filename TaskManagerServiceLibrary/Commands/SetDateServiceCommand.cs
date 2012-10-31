using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.Commands
{
    public class SetDateServiceCommand : IServiceCommand<SetDateTaskArgs>
    {
        private readonly IRepository repo;

        public SetDateServiceCommand(IRepository repo)
        {
            this.repo = repo;
        }

        public void ExecuteCommand(SetDateTaskArgs args)
        {
            var task = GetTask(args.Id);
            if(task.IsCompleted)
                throw new Exception();
            task.DueDate = args.DueDate;
        }

        private ServiceTask GetTask(int id)
        {
            return repo.Select(id);
        }
    }

    public class SetDateServiceCommandTest
    {
        private SetDateTaskArgs args = new SetDateTaskArgs {Id = 1, DueDate = DateTime.Today};
        private readonly IRepository repo = Substitute.For<IRepository>();
        readonly SetDateServiceCommand command;
        
        public SetDateServiceCommandTest()
        {
            command = new SetDateServiceCommand(repo);
        }

        [Fact]
        public void command_should_set_date_for_task()
        {
            var serviceTask = new ServiceTask {Id = 1, DueDate = default(DateTime)};
            repo.Select(1).Returns(serviceTask);

            command.ExecuteCommand(args);
            serviceTask.DueDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void should_throw_exception_if_task_is_completed()
        {
            var serviceTask = new ServiceTask { Id = 1, DueDate = default(DateTime), IsCompleted = true};
            repo.Select(1).Returns(serviceTask);

            Action action = () => command.ExecuteCommand(args);

            action.ShouldThrow<Exception>();
        }
    }
}