using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ConnectToWcf;
using EntitiesLibrary;
using FluentAssertions;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteHandlers;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class LineParser
    {
        private readonly List<ICommand> commands;

        public LineParser(List<ICommand> commands)
        {
            this.commands = commands;
        }

        public void ExecuteCommand(string input)
        {

            var args = input.Split(new[] { ' ' }, 2).ToList();

            var command = commands.FirstOrDefault(a => a.Name == args[0]);
            if (command == null)
                Console.WriteLine("No such command");

            else
                command.Execute(args.Count > 1 ? args[1].Trim(new[] { '\"', '\'' }) : string.Empty);
        }



        public class LineParserTests
        {
            private readonly LineParser lp;
            private readonly List<ICommand> commands;
            private readonly ICommand command1 = Substitute.For<ICommand>();
            private readonly ICommand command2 = Substitute.For<ICommand>();
            private readonly ICommand command3 = Substitute.For<ICommand>();

            public LineParserTests()
            {
                commands = new List<ICommand> { command1, command2, command3 };

                lp = new LineParser(commands);
            }

            [Fact]
            public void should_call_proper_command()
            {
                command1.Name.Returns("add");
                command2.Name.Returns("command");
                command3.Name.Returns("hello");

                lp.ExecuteCommand("add foo");
                command1.Received().Execute("foo");
            }

            [Fact]
            public void should_call_the_first_command()
            {
                command1.Name.Returns("add");
                command2.Name.Returns("add");

                lp.ExecuteCommand("add aaa");
                command1.Received().Execute("aaa");
            }

            [Fact]
            public void should_throw_exception_if_for_wrong_command()
            {
                command1.Name.Returns("add");
                command2.Name.Returns("command");
                command3.Name.Returns("hello");

                var sb = new StringBuilder();

                Console.SetOut(new StringWriter(sb));
                lp.ExecuteCommand("adkkkd world");
                sb.ToString().ShouldBeEquivalentTo("This command is incorrect. Please, try again!\r\n");
            }

            [Fact]
            public void should_ignore_double_quotes()
            {
                command1.Name.Returns("add");
                lp.ExecuteCommand("add \"hello world\"");
                command1.Received().Execute("hello world");
            }

            [Fact]
            public void should_ignore_single_quotes()
            {
                command1.Name.Returns("add");
                lp.ExecuteCommand("add \'hello world\'");
                command1.Received().Execute("hello world");
            }
        }
    }
}
