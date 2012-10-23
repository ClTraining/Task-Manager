using System;
using System.Collections.Generic;
using System.IO;
using ConnectToWcf;
using TaskManagerServiceLibrary;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    public abstract class Command<T> : ICommand
    {
        protected readonly IClientConnection client;
        private readonly ArgumentConverter<T> converter;
        private readonly TextWriter textWriter;

        protected Command(IClientConnection client = null, ArgumentConverter<T> converter = null,
                          TextWriter textWriter = null)
        {
            Name = GetType().Name.ToLower();
            this.client = client;
            this.converter = converter;
            this.textWriter = textWriter;
        }

        #region ICommand Members

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual void Execute(object argument)
        {
            try
            {
                object converted = /*converter == null ? argument :*/ Convert(argument);
                ExecuteWithGenericInput((T) converted);
            }
            catch (TaskNotFoundException e)
            {
                OutText("Task not found. Id = " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //OutText("Wrong arguments.");
            }
        }

        #endregion

        protected abstract void ExecuteWithGenericInput(T input);

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