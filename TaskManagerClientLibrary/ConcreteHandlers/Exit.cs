using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Exit : Command<string>
    {
        private readonly EnvironmentWrapper manager;

        public Exit(EnvironmentWrapper manager)
            : base()
        {
            this.manager = manager;
        }

        public override void Execute(object argument)
        {
            ExecuteWithGenericInput((string)argument);
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            manager.Exit();
        }
    }

    public class ExitTests
    {
        private readonly EnvironmentWrapper wrapper = Substitute.For<EnvironmentWrapper>();
        private readonly Exit handler;

        public ExitTests()
        {
            handler = new Exit (wrapper);
        }

        [Fact]
        public void should_close_application()
        {
            handler.Execute(null);

            wrapper.Received().Exit();
        }
    }
}