using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class CompleteServiceCommand : IServiceCommand<CompleteTaskArgs>
    {
        private readonly IRepository repo;

        public CompleteServiceCommand(IRepository repo)
        {
            this.repo = repo;
        }

        public void ExecuteCommand(CompleteTaskArgs args)
        {
            var task = repo.Select(args.Id);
            task.IsCompleted = true;
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

            var command = new CompleteServiceCommand(repo);
            command.ExecuteCommand(new CompleteTaskArgs { Id = 1 });
            serviceTask.IsCompleted.Should().BeTrue();
        }
    }
}