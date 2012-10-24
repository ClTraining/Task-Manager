using System.IO;
using ConnectToWcf;
using EntitiesLibrary.CommandArguments;
using NSubstitute;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Complete : Command<CompleteTaskArgs>
    {
        public Complete(IClientConnection client, ArgumentConverter<CompleteTaskArgs> converter, TextWriter textWriter)
            : base(client, converter, textWriter)
        {
            Description = "Mark task by ID as completed.";
        }

        protected override void ExecuteWithGenericInput(CompleteTaskArgs input)
        {
            client.Complete(input);
            OutText(string.Format("Task ID: {0} completed.", input.Id));
        }
    }

    public class CompleteTests
    {
        private const string taskName = "sometask1";
        private readonly IClientConnection client = Substitute.For<IClientConnection>();

        private readonly ArgumentConverter<CompleteTaskArgs> converter =
            Substitute.For<ArgumentConverter<CompleteTaskArgs>>();

        private readonly Complete handler;

        public CompleteTests()
        {
            handler = new Complete(client, converter, new StringWriter());
        }
    }
}
