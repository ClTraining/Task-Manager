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

        private List<string> GetArguments(string input)
        {
            var args = input.Split(' ').ToList();

            return new List<string> { args[0], args.Count > 1 ? string.Join(" ", args.Skip(1)) : string.Empty };
        }
        public void ExecuteCommand(string input)
        {
            var args = GetArguments(input);
            try
            {
                commands.First(a => a.Name == args[0]).Execute(args[1].Trim(new char[]{'\"'}));
            }
            catch (FaultException<ExceptionDetail> e)
            {
                Console.WriteLine("Task not found: (Id = {0})", e.Detail.Message);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("This command is incorrect. Please, try again!");
            }
            catch
            {
                Console.WriteLine("Arguments are incorrect.");
            }
        }
    }

    public class LineParserTester
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly LineParser lp;

        public LineParserTester()
        {
            lp = new LineParser(new List<ICommand> { new Add(client), new Complete(client), new List(client) });
        }

        [Fact]
        public void should_throw_exception_if_for_wrong_command()
        {
            var sb = new StringBuilder();

            Console.SetOut(new StringWriter(sb));
            lp.ExecuteCommand("adkkkd world");
            sb.ToString().ShouldBeEquivalentTo("This command is incorrect. Please, try again!\r\n");
        }

        [Fact]
        public void should_show_message_if_task_doesnt_exists()
        {
            var ID = 5;
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            var task = new ContractTask { Id = ID, IsCompleted = false, Name = "test" };
            client.GetTaskById(ID).Returns(new List<ContractTask> { task });
            lp.ExecuteCommand(string.Format("list {0}", ID));
            sb.ToString().ShouldBeEquivalentTo(string.Format("ID: {0}	Task: {1}\t\t\tCompleted: -\r\n", ID, task.Name));

        }
    }
}
