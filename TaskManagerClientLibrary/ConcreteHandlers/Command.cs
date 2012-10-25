using System.Collections.Generic;
using System.IO;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        protected readonly ArgumentConverter<T> converter;
        private readonly TextWriter textWriter;
        //protected void ExecuteWithGenericInput(T input) { }

        public abstract void Execute(List<string> argument);

        protected Command(ArgumentConverter<T> converter, TextWriter textWriter)
        {
            Name = GetType().Name.ToLower();
            this.converter = converter;
            this.textWriter = textWriter;
        }
        //try
        //{
        //    var converted = converter == null ? argument : Convert(argument);
        //    ExecuteWithGenericInput((T)converted);
        //}
        //catch (TaskNotFoundException e)
        //{
        //    OutText("Task not found.");
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e.Message);
        //}


        protected void OutText(string text)
        {
            textWriter.WriteLine(text);
        }
    }
}