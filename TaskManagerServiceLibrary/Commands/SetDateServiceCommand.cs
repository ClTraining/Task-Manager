using System;
using NSubstitute;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class SetDateServiceCommand : IServiceCommand
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }

        private readonly ITodoList todoList;

        public SetDateServiceCommand(ITodoList todoList)
        {
            this.todoList = todoList;
        }

        public void ExecuteCommand()
        {
            todoList.SetTaskDate(Id, DueDate);
        }
    }

    public class SetDateServiceCommandTest
    {
        private readonly ITodoList todoList = Substitute.For<ITodoList>();
        private readonly SetDateServiceCommand command;

        public SetDateServiceCommandTest()
        {
            command = new SetDateServiceCommand(todoList) { Id = 1, DueDate = DateTime.Today };
        }

        [Fact]
        public void command_should_set_date_for_task()
        {
            command.ExecuteCommand();
            todoList.SetTaskDate(1, DateTime.Today);
        }
    }
}