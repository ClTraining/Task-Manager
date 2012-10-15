using System;
using ConnectToWcf;
using FluentAssertions;
using NSubstitute;
using TaskManagerServiceLibrary;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Exit : Command<string>
    {
        private readonly ExitManager manager;

        public Exit(IClientConnection client, ArgumentConverter<string> converter, ExitManager manager)
            : base(client, typeof(Exit), converter)
        {
            this.manager = manager;
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            manager.Close();
        }
    }

    public class ExitTests
    {
        private readonly ArgumentConverter<string> converter = Substitute.For<ArgumentConverter<string>>();
        private readonly IClientConnection client = Substitute.For<IClientConnection>();
        private readonly ExitManager manager = Substitute.For<ExitManager>();
        private readonly Exit handler;

        public ExitTests()
        {
            handler = new Exit(client, converter, manager);
        }

        [Fact]
        public void should_close_application()
        {
            converter.Convert(null).Returns((string)null);
            handler.Execute(null);

            manager.Received().Close();
        }
    }
}