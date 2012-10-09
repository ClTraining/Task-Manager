using System;
using ConnectToWcf;
using FluentAssertions;
using Xunit;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        private readonly ArgumentConverter<T> converter;
        protected readonly IClientConnection client;

        public string Name { get; set; }

        protected Command(IClientConnection client, Type derived)
        {
            this.client = client;
            Name = derived.Name.ToLower();
            converter = new ArgumentConverter<T>();
        }

        protected abstract void Execute(T input);

        public void Execute(object argument)
        {
            var converted = Convert(argument);
            Execute((T)converted);
        }

        public object Convert(object input)
        {
            return converter.Convert((string)input);
        }
    }
}
