using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class ClearDate: Command<ClearDateArgs>
    {
        public ClearDate(IClientConnection client, ArgumentConverter<ClearDateArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
            Description = "Clears due date for specified task by ID.";
        }

        protected override void ExecuteWithGenericInput(ClearDateArgs input)
        {
            client.ClearTaskDueDate(input);
            OutText(string.Format("Due date cleared for task ID: {0} .", input.Id));
        }
    }

    public class ClearDateTests
    {
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ArgumentConverter<ClearDateArgs> converter = Substitute.For<ArgumentConverter<ClearDateArgs>>();
        private readonly ClearDate handler;

        public ClearDateTests()
        {
            handler = new ClearDate(client, converter, new StringWriter());
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
