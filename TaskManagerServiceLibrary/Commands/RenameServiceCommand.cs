using System;
using CommandQueryLibrary.ServiceSpecifications;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.Commands
{
    public class RenameServiceCommand : IServiceCommand<RenameTaskArgs>
    {
        private readonly IRepository repo;

        public RenameServiceCommand(IRepository repo)
        {
            this.repo = repo;
        }

        public void ExecuteCommand(RenameTaskArgs args)
        {
            var task = GetTask(args.Id);
            task.Name = args.Name;
        }

        private ServiceTask GetTask(int id)
        {
            return repo.Select(id);
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

            var command = new RenameServiceCommand(repo);
            command.ExecuteCommand(new RenameTaskArgs {Id = 1, Name = "task1"});

            serviceTask.Name.Should().Be("task1");
        }
    }
}