using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers.HelpCommand
{
    public class HelpDisplayer : IHelpDisplayer
    {
        #region IHelpDisplayer Members

        public void Show(ICommand command)
        {
            Console.WriteLine("  " + command.Name + "\n\t" + command.Description + "\n");
        }

        #endregion
    }

    public class DisplayHelpTests
    {
        [Fact]
        public void should_test_show_task_name_and_description()
        {
            var command = Substitute.For<ICommand>();
            command.Name = "help";
            command.Description = "task help";

            var displayHelp = new HelpDisplayer();
            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));
            displayHelp.Show(command);
            sb.ToString().Should().Be("  help\n\ttask help\n\r\n");
        }
    }
}