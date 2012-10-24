using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Exit : ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private readonly EnvironmentWrapper manager;

        public Exit(EnvironmentWrapper manager)
        {
            Name = GetType().Name.ToLower();
            this.manager = manager;
            Description = "Exits from client.";
        }

        public void Execute(object argument)
        {
            manager.Exit();
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