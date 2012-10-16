using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.DisplayResultClasses
{
    public class DisplayHelp : IDisplayHelp
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

            var displayHelp = new DisplayHelp();
            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));
            displayHelp.Show(commands);
            sb.ToString().Should().Be("  " + command.Name + "\n\t" + command.Description + "\n\r\n");
        }
    }
}