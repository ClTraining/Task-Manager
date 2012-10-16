using System;
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
        private readonly ICommand command = Substitute.For<ICommand>();
        [Fact]
        public void should_test_show_method()
        {
            command.Name = "help";
            command.Description = "task help";

            var displayHelp = new DisplayHelp();
            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));
            displayHelp.Show(command);
            sb.ToString().Should().Be("  " + command.Name + "\n\t" + command.Description + "\n\r\n");
        }
    }
}