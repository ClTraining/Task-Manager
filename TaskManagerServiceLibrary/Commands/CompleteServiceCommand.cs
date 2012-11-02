using NSubstitute;
using TaskManagerServiceLibrary.ToDoList;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class CompleteServiceCommand : IServiceCommand
    {
        public int Id { get; set; }

        private readonly ITodoList todoList;

        public CompleteServiceCommand(ITodoList todoList)
        {
            this.todoList = todoList;
        }

        public void ExecuteCommand()
        {
            todoList.CompleteTask(Id);
        }
    }

    public class CompleteServiceCommandTests
    {
        private readonly ITodoList todoList = Substitute.For<ITodoList>();
        private readonly CompleteServiceCommand command;

        public CompleteServiceCommandTests()
        {
            command = new CompleteServiceCommand(todoList){Id = 1};
        }

        [Fact]
        public void command_should_complete_task()
        {
            command.ExecuteCommand();
            todoList.Received().CompleteTask(1);
        }
    }
}