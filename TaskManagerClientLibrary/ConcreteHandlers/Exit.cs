using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Exit : ICommand
    {
        public string Name { get { return GetType().Name.ToLower(); } }
        public string Description { get; private set; }
        private readonly EnvironmentWrapper manager;

        public void Execute(List<string> argument)
        {
            manager.Exit();
        }

        public Exit(EnvironmentWrapper manager)
        {
            this.manager = manager;
            Description = "Exits from client.";
        }
    }

    public class ExitTests
    {
        private readonly Exit handler;
        private readonly EnvironmentWrapper wrapper = Substitute.For<EnvironmentWrapper>();

        public ExitTests()
        {
            handler = new Exit(wrapper);
        }

        [Fact]
        public void should_close_application()
        {
            handler.Execute(null);

            wrapper.Received().Exit();
        }
    }
}