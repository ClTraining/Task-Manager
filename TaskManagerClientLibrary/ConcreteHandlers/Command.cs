using System;
using System.IO;
using ConnectToWcf;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        private readonly ArgumentConverter<T> converter;
        protected TextWriter textWriter;
        protected readonly IClientConnection client;

        public string Name { get; private set; }

        protected Command(IClientConnection client, Type derived, ArgumentConverter<T> converter, TextWriter textWriter)
        {
            this.client = client;
            Name = derived.Name.ToLower();
            this.converter = converter;
            this.textWriter = textWriter;
        }

        public abstract void ExecuteWithGenericInput(T input);

        public void Execute(object argument)
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
