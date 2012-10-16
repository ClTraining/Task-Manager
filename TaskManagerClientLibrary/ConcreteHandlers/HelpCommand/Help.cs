using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public class Help : Command<string>
    {
        private readonly IDisplayHelp display;
        private readonly ICommandsProvider provider;

        public Help(IDisplayHelp display, ICommandsProvider provider)
        {
            Name = "?";
            this.display = display;
            this.provider = provider;
            Description = "Causes help";
        }

        public override void Execute(object argument)
        {
            ExecuteWithGenericInput((string)argument);
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            foreach (var command in provider.Commands)
                display.Show(command);    
        }
    }

    public class HelpTests
    {
        private readonly ICommandsProvider provider = Substitute.For<ICommandsProvider>();
        private readonly IDisplayHelp display = Substitute.For<IDisplayHelp>();
        private readonly ICommand command = Substitute.For<ICommand>();
        readonly Help help;

        public HelpTests()
        {
            help = new Help(display, provider);
        }

        [Fact]
        public void execute_method_test()
        {
            IEnumerable<ICommand> commands = new List<ICommand> {command};
            provider.Commands.Returns(commands);
            help.Execute(null);
            foreach (var c in commands)
                display.Received().Show(c);
        }
    }
}
