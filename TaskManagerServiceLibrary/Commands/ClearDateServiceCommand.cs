using System;
using EntitiesLibrary;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using TaskManagerServiceLibrary.Repositories;
using Xunit;
using FluentAssertions;

namespace TaskManagerServiceLibrary.Commands
{
    public class ClearDateServiceCommand : IServiceCommand
    {
        private int Id { get; set; }

        public ServiceTask ExecuteCommand(ServiceTask task)
        {
            task.DueDate = default(DateTime);
            return task;
        }
    }

    public class ClearDateServiceCommandTests
    {
        [Fact]
        public void command_should_clear_date()
        {
            var serviceTask = new ServiceTask {Id = 1, DueDate = DateTime.Today};
            var repo = new MemoRepository();
            repo.AddTask(new AddTaskArgs {Name = "some", DueDate = DateTime.Today});

        }
    }
}