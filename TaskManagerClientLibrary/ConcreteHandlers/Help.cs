using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
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

        readonly Help help;

        public HelpTests()
        {
            help = new Help(kernel, display);
        }

        [Fact]
        public void execute_method_test()
        {
            var list = new List<ICommand>{help};
            kernel.GetAll<ICommand>().Returns(list);
            help.Execute(null);
        }
    }
}
