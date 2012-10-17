using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public class HelpDisplayer : IHelpDisplayer
    {
        public void Show(ICommand command)
        {
            Console.WriteLine("  " + command.Name + "\n\t" + command.Description + "\n");
        }
    }

    public class DisplayHelpTests
    {
        private readonly List<ICommand> commands = Substitute.For<List<ICommand>>();
        [Fact]
        public void should_test_show_method()
        {
            var command = Substitute.For<ICommand>();
            command.Name = "help";
            command.Description = "task help";
            commands.Add(command);

            var displayHelp = new HelpDisplayer();
            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));
            displayHelp.Show(command);
            sb.ToString().Should().Be("  help\n\ttask help\n\r\n");
        }
    }
}