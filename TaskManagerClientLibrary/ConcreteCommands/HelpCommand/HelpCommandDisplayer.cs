using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteCommands.HelpCommand
{
    public class HelpCommandDisplayer : IHelpCommandDisplayer
    {
        public void Show(ICommand command)
        {
            Console.WriteLine("  " + command.Name + "\n\t" + command.Description + "\n");
        }
    }

    public class DisplayHelpTests
    {
        [Fact]
        public void should_test_show_task_name_and_description()
        {
            var command = Substitute.For<ICommand>();
            command.Name.Returns("help");
            command.Description.Returns("task help");

            var displayHelp = new HelpCommandDisplayer();
            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));
            displayHelp.Show(command);
            sb.ToString().Should().Be("  help\n\ttask help\n\r\n");
        }
    }
}