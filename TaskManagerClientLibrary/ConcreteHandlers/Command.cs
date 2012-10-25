using System;
using System.Collections.Generic;
using System.IO;
using EntitiesLibrary;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private readonly ArgumentConverter<T> converter;
        private readonly TextWriter textWriter;

        protected Command(ArgumentConverter<T> converter, TextWriter textWriter)
        {
            Name = GetType().Name.ToLower();
            this.converter = converter;
            this.textWriter = textWriter;
        }

        public virtual void Execute(object argument)
        {
            try
            {
                var converted = converter == null ? argument : Convert(argument);
                ExecuteWithGenericInput((T)converted);
            }
            catch (TaskNotFoundException e)
            {
                OutText("Task not found.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected virtual void ExecuteWithGenericInput(T input) { }

        protected void OutText(string text)
        {
            textWriter.WriteLine(text);
        }

        private object Convert(object input)
        {
            return converter.Convert(input as List<string>);
        }
    }
}