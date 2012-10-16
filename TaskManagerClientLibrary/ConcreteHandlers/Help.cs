using System.Collections.Generic;
using NSubstitute;
using Ninject;
using TaskManagerClientLibrary.ConcreteHandlers.DisplayResultClasses;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Help : Command<string>
    {
        private readonly IKernel kernel;
        private readonly IDisplayHelp display;

        public Help(IKernel kernel, IDisplayHelp display)
            : base("?")
        {
            this.kernel = kernel;
            this.display = display;
            Description = "Causes help";
        }

        public override void Execute(object argument)
        {
            ExecuteWithGenericInput((string) argument);
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            var commands = kernel.GetAll<ICommand>();

            foreach (var command in commands)
                display.Show(command);
        }
    }

    public class HelpTests
    {
        private readonly IKernel kernel = Substitute.For<IKernel>();
        private readonly IDisplayHelp display = Substitute.For<IDisplayHelp>();
        private readonly ICommand command = Substitute.For<ICommand>();
        readonly Help help;

        public HelpTests()
        {
            help = new Help(kernel, display);
        }

        [Fact]
        public void execute_method_test()
        {
            IEnumerable<ICommand> commands = new List<ICommand> {command};
            kernel.GetAll<ICommand>().Returns(commands);
            help.Execute(null);
            display.Received().Show(command);
        }
    }
}
