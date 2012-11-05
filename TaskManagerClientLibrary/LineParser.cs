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

            if (!args.Any())
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
                catch (WrongTaskArgumentsException e)
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
        private const string EndForStringBuilder = "\r\n";

        public LineParserTests()
        {
            commands = new List<ICommand> { command1, command2, command3 };

            lp = new LineParser(commands, parser);
        }

        [Fact]
        public void should_call_proper_command()
        {
            const string command1Name = "add";
            const string command2Name = "command";
            const string command3Name = "hello";
            const string arg = "foo";
            const string input = command1Name+" " +arg;
            var list = new List<string> { command1Name, arg };

            command1.Name.Returns(command1Name);
            command2.Name.Returns(command2Name);
            command3.Name.Returns(command3Name);
            
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);
            list.RemoveAt(0);
            command1.Received().Execute(list);
        }

        [Fact]
        public void should_call_the_first_command()
        {
            const string commandName = "add";
            command1.Name.Returns(commandName);
            command2.Name.Returns(commandName);
            const string arg = "aaa";
            const string input = commandName + " " + arg;
            var list = new List<string> { commandName, arg };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);
            list.RemoveAt(0);
            command1.Received().Execute(list);
            command2.DidNotReceiveWithAnyArgs().Execute(list);
        }

        [Fact]
        public void should_inform_if_no_such_command()
        {
            const string command1Name = "add";
            const string command2Name = "command";
            const string command3Name = "hello";
            const string wrongArgs = "aaa";
            const string wrongCommand = "ababa";
            const string someArgs = "bababab";
            const string input = wrongCommand + " " + someArgs;

            command1.Name.Returns(command1Name);
            command2.Name.Returns(command2Name);
            command3.Name.Returns(command3Name);
            
            var list = new List<string> { wrongCommand, someArgs };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);

            
            command1.DidNotReceiveWithAnyArgs().Execute(new List<string> { wrongArgs });
            command2.DidNotReceiveWithAnyArgs().Execute(new List<string> { wrongArgs });
            command3.DidNotReceiveWithAnyArgs().Execute(new List<string> { wrongArgs });
        }

        [Fact]
        public void for_wrong_command_should_inform_user_that_command_does_not_exists()
        {
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            const string command1Name = "add";
            const string command2Name = "command";
            const string command3Name = "abrakadabra";
            const string world = "world";
            const string hello = "hello";
            const string helloWorld = hello + " " + world;
            const string input = helloWorld;
            const string noSuchCommand = "No such command";

            command1.Name.Returns(command1Name);
            command2.Name.Returns(command2Name);
            command3.Name.Returns(command3Name);

            
            var list = new List<string> { hello, world };
            parser.Parse(input).Returns(list);

            lp.ExecuteCommand(input);


            sb.ToString().ShouldBeEquivalentTo(noSuchCommand + EndForStringBuilder);
        }

        [Fact]
        public void should_ignore_double_quotes()
        {
            const string add = "add";
            const string helloWorld = "hello world";
            const string input = add +"\""+helloWorld+"\"";

            command1.Name.Returns(add);
            var list = new List<string> { add, helloWorld };
            parser.Parse(input).Returns(list);
            lp.ExecuteCommand(input);
            list.RemoveAt(0);

            command1.Received().Execute(list);
        }

        [Fact]
        public void should_inform_if_could_not_set_date()
        {
            const string world = "world";
            const string hello = "hello";
            const string helloWorld = hello + " " + world;
            const string taskNotFoundId = "Task not found, ID: 1";

            var list = new List<string> { hello, world };
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            command1.Name.Returns(hello);
            parser.Parse(helloWorld).Returns(list);
            command1.When(x => x.Execute(list)).Do(x => { throw new TaskNotFoundException(1); });

            lp.ExecuteCommand(helloWorld);

            sb.ToString().Should().Be(taskNotFoundId + EndForStringBuilder);
        }

        [Fact]
        public void should_inform_if_task_was_not_found()
        {
            const string world = "world";
            const string hello = "hello";
            const string helloWorld = hello +" "+ world;
            const string exception = "exception";

            var list = new List<string> { hello, world };
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            command1.Name.Returns(hello);
            
            parser.Parse(helloWorld).Returns(list);
            command1.When(x => x.Execute(list)).Do(x =>
                                                       {
                                                           throw new CouldNotSetDateException(exception);
                                                       });

            lp.ExecuteCommand(helloWorld);

            sb.ToString().Should().Be(exception + EndForStringBuilder);
        }

        [Fact]
        public void should_handle_wrong_arguments_exception()
        {
            const string commandName = "command";
            const string argumentName = "argument";
            const string commandArgument = commandName + " " + argumentName;
            const string wrongCommandArguments = "Wrong command arguments.";

            var input = new List<string> {commandName, argumentName};
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            command1.Name.Returns(commandName);
            parser.Parse(commandArgument).Returns(input);
            command1.When(x => x.Execute(input)).Do(x =>
                                                        {
                                                            throw new WrongTaskArgumentsException(wrongCommandArguments);
                                                        });
            lp.ExecuteCommand(commandArgument);

            sb.ToString().Should().Be(wrongCommandArguments + EndForStringBuilder);
        }

        [Fact]
        public void should_do_nothing_if_empty_string_received()
        {
            const string empty = "";
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));

            parser.Parse(empty).Returns(new List<string>());
            lp.ExecuteCommand(empty);

            sb.ToString().Should().Be(empty);
        }
    }
}