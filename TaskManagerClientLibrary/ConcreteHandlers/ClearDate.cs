using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class ClearDate : Command<ClearDateArgs>
    {
        private IClientConnection Client { get; set; }

        public ClearDate(ArgumentConverter<ClearDateArgs> converter, TextWriter textWriter, IClientConnection client)
            : base(converter, textWriter)
        {
            Client = client;
            Description = "Clears due date for specified task by ID.";
        }

        public override void Execute(List<string> argument)
        {
            var clearDateArgs = converter.Convert(argument);
            Client.ClearTaskDueDate(clearDateArgs);
            OutText(string.Format("Due date cleared for task ID: {0} .", clearDateArgs.Id));
        }
    }

    public class ClearDateTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<ClearDateArgs> converter = Substitute.For<ArgumentConverter<ClearDateArgs>>();
        private readonly ClearDate handler;

        public ClearDateTests()
        {
            handler = new ClearDate(converter, new StringWriter(), client);
        }

        [Fact]
        public void should_send_string_return_id()
        {
            var arguments = new List<string> { "12" };
            var clearDateArgs = new ClearDateArgs { Id = 12 };
            converter.Convert(arguments).Returns(clearDateArgs);
            handler.Execute(arguments);
            client.Received().ClearTaskDueDate(clearDateArgs);
        }
    }
}