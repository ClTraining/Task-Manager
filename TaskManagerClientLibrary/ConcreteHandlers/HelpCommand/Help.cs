using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public class Help : Command<string>
    {
        private readonly IDisplayHelp display;

        public Help(IDisplayHelp display)
        {
            Name = "?";
            this.display = display;
            Description = "Causes help";
        }

        public override void Execute(object argument)
        {
            ExecuteWithGenericInput((string)argument);
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            foreach (var command in CommandContainer.Commands)
                display.Show(command);    
        }
    }

    public class HelpTests
    {
        private readonly ICommandContainer commands = Substitute.For<ICommandContainer>();
        private readonly IDisplayHelp display = Substitute.For<IDisplayHelp>();
        private readonly ICommand command = Substitute.For<ICommand>();
        readonly Help help;

        public HelpTests()
        {
            help = new Help(display);
        }

        [Fact]
        public void execute_method_test()
        {
            IEnumerable<ICommand> commands = new List<ICommand> {command};
            this.commands.GetCommands().Returns(commands);
            help.Execute(null);
            foreach (var c in commands)
                display.Received().Show(c);
        }
    }
}
