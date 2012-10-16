using ConnectToWcf;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        private readonly ArgumentConverter<T> converter;
        protected readonly IClientConnection client;

        public string Name { get; private set; }

        protected Command() : this(null, null) { }
        protected Command(IClientConnection client, ArgumentConverter<T> converter)
        {
            this.client = client;
            Name = GetType().Name.ToLower();
            this.converter = converter;
        }


        protected abstract void ExecuteWithGenericInput(T input);

        public virtual void Execute(object argument)
        {
            var converted = Convert(argument);
            ExecuteWithGenericInput((T)converted);
        }

        private object Convert(object input)
        {
            return converter.Convert((string)input);
        }
    }
}
