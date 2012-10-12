using System.IO;
using ConnectToWcf;
using TaskManagerServiceLibrary;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        private readonly ArgumentConverter<T> converter;
        private readonly TextWriter textWriter;
        protected readonly IClientConnection client;

        public string Name
        {
            get { return GetType().Name.ToLower(); }
        }

        protected Command(IClientConnection client, ArgumentConverter<T> converter, TextWriter textWriter)
        {
            this.client = client;
            this.converter = converter;
            this.textWriter = textWriter;
        }

        public void Execute(object argument)
        {
            try
            {
                var converted = Convert(argument);
                ExecuteWithGenericInput((T)converted);
            }
            catch (TaskNotFoundException e)
            {
                OutText(e.Message);
            }
        }

        protected abstract void ExecuteWithGenericInput(T input);

        protected void OutText(string text)
        {
            textWriter.WriteLine(text);
        }

        private object Convert(object input)
        {
            return converter.Convert((string)input);
        }
    }
}
