using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var args = ParceInput(input);

            var command = commands.FirstOrDefault(a => a.Name == args[0]);
            if (command == null)
                Console.WriteLine("No such command");
            else
            {
                args.RemoveAt(0);
                command.Execute(args);
            }
        }

        private List<string> ParceInput(string input)
        {
            var inputArr = input.Split(new[] {' '}, 2);
            var arguments = new List<string> {inputArr[0]};

            if (inputArr.Count() > 1)
            {
                var argumentsStr = inputArr[1];
                const string pattern = "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)";
                var exp = new Regex(pattern);
                arguments.AddRange(exp.Split(argumentsStr));
            }

            var parceInput =
                arguments.Where(s => !String.IsNullOrEmpty(s)).Select(s => s.Trim(new[] {'\'', '\"', ' ', '\t'})).ToList
                    ();
            return parceInput;
        }
    }

    public class LineParserTests
    {
        private readonly ICommand command1 = Substitute.For<ICommand>();
        private readonly ICommand command2 = Substitute.For<ICommand>();
        private readonly ICommand command3 = Substitute.For<ICommand>();
        private readonly List<ICommand> commands;
        private readonly LineParser lp;

        public LineParserTests()
        {
            commands = new List<ICommand> {command1, command2, command3};

            lp = new LineParser(commands);
        }

        [Fact]
        public void should_call_proper_command()
        {
            command1.Name.Returns("add");
            command2.Name.Returns("command");
            command3.Name.Returns("hello");

            lp.ExecuteCommand("add foo");
            command1.ReceivedWithAnyArgs().Execute(new List<string> {"foo"});
        }

        [Fact]
        public void should_call_the_first_command()
        {
            command1.Name.Returns("add");
            command2.Name.Returns("add");

            lp.ExecuteCommand("add aaa");

            command1.ReceivedWithAnyArgs().Execute(new List<string> {"aaa"});
            command2.DidNotReceiveWithAnyArgs().Execute("aaa");
        }

        [Fact]
        public void should_inform_if_no_such_command()
        {
            command1.Name.Returns("add");
            command2.Name.Returns("command");
            command3.Name.Returns("hello");

            lp.ExecuteCommand("ababa bababab");

            command1.DidNotReceiveWithAnyArgs().Execute("aaa");
            command2.DidNotReceiveWithAnyArgs().Execute("aaa");
            command3.DidNotReceiveWithAnyArgs().Execute("aaa");
        }

        [Fact]
        public void for_wrong_command_should_inform_user_that_command_does_not_exists()
        {
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            command1.Name.Returns("add");
            command2.Name.Returns("command");
            command3.Name.Returns("abrakadabra");

            lp.ExecuteCommand("hello world");

            sb.ToString().ShouldBeEquivalentTo("No such command\r\n");
        }

        [Fact]
        public void should_ignore_double_quotes()
        {
            command1.Name.Returns("add");
            lp.ExecuteCommand("add \"hello world\"");
            var argument = new List<string> {"hello world"};
            command1.ReceivedWithAnyArgs().Execute(argument);
        }

        [Fact]
        public void should_ignore_single_quotes()
        {
            command1.Name.Returns("add");
            lp.ExecuteCommand("add \'hello world\'");
            var argument = new List<string> {"hello world"};
            command1.ReceivedWithAnyArgs().Execute(argument);
        }
    }
}