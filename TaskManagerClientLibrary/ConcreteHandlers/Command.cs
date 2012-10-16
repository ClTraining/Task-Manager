using ConnectToWcf;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        private readonly ArgumentConverter<T> converter;
        protected readonly IClientConnection client;

        public string Name { get; set; }
        public string Description { get; set; }

        protected Command(string name)
        {
            Name = name;
        }
        protected Command(IClientConnection client, string name, ArgumentConverter<T> converter)
        {
            this.client = client;
            this.converter = converter;
            Name = name;
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
