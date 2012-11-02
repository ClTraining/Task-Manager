using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NSubstitute;
using TaskManagerClientLibrary.ConcreteCommands;
using Xunit;

namespace TaskManagerClientLibrary
{
    public class LineParser
    {
        private readonly List<ICommand> commands;
        private readonly InputParser parser;

        public LineParser(List<ICommand> commands, InputParser parser)
        {
            this.commands = commands;
            this.parser = parser;
        }

        public void ExecuteCommand(string input)
        {
                var args = parser.Parse(input);

                var command = commands.FirstOrDefault(a => a.Name == args[0]);

                if (command == null)
                    Console.WriteLine("No such command");
                else
                {
                    var skip = args.Skip(1).ToList();
                    command.Execute(skip);
                }

        }
    }

    public class LineParserTests
    {
        private readonly ICommand command1 = Substitute.For<ICommand>();
        private readonly ICommand command2 = Substitute.For<ICommand>();
        private readonly ICommand command3 = Substitute.For<ICommand>();
        private readonly List<ICommand> commands;
        private readonly LineParser lp;
        private readonly InputParser parser = Substitute.For<InputParser>();

        public LineParserTests()
        {
            commands = new List<ICommand> { command1, command2, command3 };

            lp = new LineParser(commands, parser);
        }

        [Fact]
        public void should_call_proper_command()
        {
            command1.Name.Returns("add");
            command2.Name.Returns("command");
            command3.Name.Returns("hello");
            const string input = "add foo";
            var list = new List<string> { "add", "foo" };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);
            list.RemoveAt(0);
            command1.Received().Execute(list);
        }

        [Fact]
        public void should_call_the_first_command()
        {
            command1.Name.Returns("add");
            command2.Name.Returns("add");
            const string input = "add aaa";
            var list = new List<string> { "add", "aaa" };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);
            list.RemoveAt(0);
            command1.Received().Execute(list);
            command2.DidNotReceiveWithAnyArgs().Execute(list);
        }

        [Fact]
        public void should_inform_if_no_such_command()
        {
            command1.Name.Returns("add");
            command2.Name.Returns("command");
            command3.Name.Returns("hello");

            const string input = "ababa bababab";
            var list = new List<string> { "ababa", "bababab" };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);

            command1.DidNotReceiveWithAnyArgs().Execute(new List<string> { "aaa" });
            command2.DidNotReceiveWithAnyArgs().Execute(new List<string> { "aaa" });
            command3.DidNotReceiveWithAnyArgs().Execute(new List<string> { "aaa" });
        }

        [Fact]
        public void for_wrong_command_should_inform_user_that_command_does_not_exists()
        {
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            command1.Name.Returns("add");
            command2.Name.Returns("command");
            command3.Name.Returns("abrakadabra");

            const string input = "hello world";
            var list = new List<string> { "hello", "world" };
            parser.Parse(input).Returns(list);

            lp.ExecuteCommand(input);

            sb.ToString().ShouldBeEquivalentTo("No such command\r\n");
        }

        [Fact]
        public void should_ignore_double_quotes()
        {
            command1.Name.Returns("add");
            const string input = "add \"hello world\"";
            var list = new List<string> { "add", "hello world" };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand("add \"hello world\"");
            list.RemoveAt(0);
            command1.ReceivedWithAnyArgs().Execute(list);
        }
    }
}