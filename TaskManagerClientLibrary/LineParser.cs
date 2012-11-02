using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EntitiesLibrary;
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

            if (args.Count == 0)
                return;

            var command = commands.FirstOrDefault(a => a.Name == args[0]);

            if (command == null)
                Console.WriteLine("No such command");
            else
            {
                try
                {
                    args.RemoveAt(0);
                    command.Execute(args);
                }
                catch (TaskNotFoundException e)
                {
                    Console.WriteLine("Task not found, ID: " + e.Message);
                }
                catch (CouldNotSetDateException e)
                {
                    Console.WriteLine(e.Message);
                }
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
            var input = "add foo";
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
            var input = "add aaa";
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

            var input = "ababa bababab";
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

            var input = "hello world";
            var list = new List<string> { "hello", "world" };
            parser.Parse(input).Returns(list);

            lp.ExecuteCommand(input);

            sb.ToString().ShouldBeEquivalentTo("No such command\r\n");
        }

        [Fact]
        public void should_ignore_double_quotes()
        {
            command1.Name.Returns("add");
            var input = "add \"hello world\"";
            var list = new List<string> { "add", "hello world" };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand("add \"hello world\"");
            list.RemoveAt(0);
            command1.Received().Execute(list);
        }

        [Fact]
        public void should_inform_if_could_not_set_date()
        {
            var world = "world";
            var list = new List<string> { "hello", world };
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            command1.Name.Returns("hello");
            parser.Parse("hello world").Returns(list);
            command1.WhenForAnyArgs(x => x.Execute(new List<string>())).Do(x => { throw new TaskNotFoundException(1); });

            lp.ExecuteCommand("hello world");

            sb.ToString().Should().Be("Task not found, ID: 1\r\n");
        }

        [Fact]
        public void should_inform_if_task_was_not_found()
        {
            var world = "world";
            var list = new List<string> { "hello", world };
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            command1.Name.Returns("hello");
            parser.Parse("hello world").Returns(list);
            command1.WhenForAnyArgs(x => x.Execute(new List<string>())).Do(x => { throw new CouldNotSetDateException("exception"); });

            lp.ExecuteCommand("hello world");

            sb.ToString().Should().Be("exception\r\n");
        }

    }
}