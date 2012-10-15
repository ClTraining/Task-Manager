using NSubstitute;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public class Exit : Command<string>
    {
        private readonly EnvironmentWrapper manager;

        public Exit(ArgumentConverter<string> converter, EnvironmentWrapper manager)
            : base(typeof(Exit), converter)
        {
            this.manager = manager;
        }

        protected override void ExecuteWithGenericInput(string input)
        {
            manager.Exit();
        }
    }

    public class ExitTests
    {
        private readonly ArgumentConverter<string> converter = Substitute.For<ArgumentConverter<string>>();
        private readonly EnvironmentWrapper manager = Substitute.For<EnvironmentWrapper>();
        private readonly Exit handler;

        public ExitTests()
        {
            handler = new Exit(converter, manager);
        }

        [Fact]
        public void should_close_application()
        {
            converter.Convert(null).Returns((string)null);
            handler.Execute(null);

            manager.Received().Exit();
        }
    }
}