using NSubstitute;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class ClearDateServiceCommand : IServiceCommand
    {
        public int Id { get; set; }

        private readonly ITodoList todoList;

        public ClearDateServiceCommand(ITodoList todoList)
        {
            this.todoList = todoList;
        }

        public void ExecuteCommand()
        {
            todoList.ClearDate(Id);
        }
    }

    public class ClearDateServiceCommandTests
    {
        private readonly ITodoList todoList = Substitute.For<ITodoList>();
        private readonly ClearDateServiceCommand command;

        public ClearDateServiceCommandTests()
        {
            command = new ClearDateServiceCommand(todoList) {Id = 1};
        }

        [Fact]
        public void command_should_clear_date()
        {
            command.ExecuteCommand();
            todoList.Received().ClearDate(1);
        }
    }
}