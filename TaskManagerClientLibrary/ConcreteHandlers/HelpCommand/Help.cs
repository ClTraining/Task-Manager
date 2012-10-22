using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public class Help : Command<List<string>>
    {
        private readonly ICommandContainer commands;
        private readonly IHelpDisplayer display;

        public Help(IHelpDisplayer display, ICommandContainer commands)
        {
            Name = "?";
            this.commands = commands;
            this.display = display;
            Description = "Causes help.";
        }

        protected override void ExecuteWithGenericInput(List<string> input)
        {
            foreach (var command in commands.GetCommands())
                display.Show(command);
        }
    }

    public class HelpTests
    {
        private readonly ICommand command = Substitute.For<ICommand>();
        private readonly ICommandContainer container = Substitute.For<ICommandContainer>();
        private readonly IHelpDisplayer display = Substitute.For<IHelpDisplayer>();
        private readonly Help help;

        public HelpTests()
        {
            help = new Help(display, container);
        }

        [Fact]
        public void execute_method_test()
        {
            var commands = new List<ICommand> {command};
            container.GetCommands().Returns(commands);
            help.Execute(null);
            foreach (var c in commands)
                display.Received().Show(c);
        }
    }
}