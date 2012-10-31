using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.Commands
{
    public class ClearDateServiceCommand : IServiceCommand<ClearDateTaskArgs>
    {
        private readonly IRepository repo;

        public ClearDateServiceCommand(IRepository repo)
        {
            this.repo = repo;
        }

        public void ExecuteCommand(ClearDateTaskArgs args)
        {
            var task = repo.Select(args.Id);
            task.DueDate = default(DateTime);
        }
    }

    public class ClearDateServiceCommandTests
    {
        [Fact]
        public void command_should_clear_date()
        {
            var repo = Substitute.For<IRepository>();
            var serviceTask = new ServiceTask {Id = 1, DueDate = DateTime.Today};
            repo.Select(1).Returns(serviceTask);

            var command = new ClearDateServiceCommand(repo);
            command.ExecuteCommand(new ClearDateTaskArgs {Id = 1});
            serviceTask.DueDate.Should().Be(default(DateTime));
        }
    }
}