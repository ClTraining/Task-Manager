using System.Collections.Generic;
using NSubstitute;
using TaskManagerClientLibrary.CommandContainer;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands.HelpCommand
{
    public class HelpCommand : ICommand
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        private readonly ICommandContainer commands;
        private readonly IHelpCommandDisplayer display;

        public void Execute(List<string> argument)
        {
            foreach (var command in commands.GetCommands())
                display.Show(command);
        }

        public HelpCommand(IHelpCommandDisplayer display, ICommandContainer commands)
        {
            Name = "?";
            this.commands = commands;
            this.display = display;
            Description = "Causes help.";
        }
    }

    public class HelpTests
    {

        private readonly ICommand command = Substitute.For<ICommand>();
        private readonly ICommandContainer container = Substitute.For<ICommandContainer>();
        private readonly IHelpCommandDisplayer display = Substitute.For<IHelpCommandDisplayer>();
        private readonly HelpCommand help;

        public HelpTests()
        {
            help = new HelpCommand(display, container);
        }

        [Fact]
        public void execute_method_test()
        {
            var commands = new List<ICommand> { command };
            container.GetCommands().Returns(commands);
            help.Execute(null);
            foreach (var c in commands)
                display.Received().Show(c);
        }
    }
}