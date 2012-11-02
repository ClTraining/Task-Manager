using NSubstitute;
using Xunit;

namespace TaskManagerServiceLibrary.Commands
{
    public class RenameServiceCommand : IServiceCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private readonly ITodoList todoList;
        
        public RenameServiceCommand(ITodoList todoList)
        {
            this.todoList = todoList;
        }

        public void ExecuteCommand()
        {
            todoList.RenameTask(Id, Name);
        }
    }

    public class RenameServiceCommandTests
    {
        private readonly RenameServiceCommand command;
        private readonly ITodoList todoList = Substitute.For<ITodoList>();

        public RenameServiceCommandTests()
        {
            command = new RenameServiceCommand(todoList) {Id = 1 , Name = "new task"};
        }

        [Fact]
        public void command_should_rename_task()
        {
            command.ExecuteCommand();
            todoList.Received().RenameTask(1, "new task");
        }
    }
}