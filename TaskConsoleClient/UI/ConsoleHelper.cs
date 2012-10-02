using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EntitiesLibrary;
using FluentAssertions;
using TaskConsoleClient.ConreteHandlers;
using TaskConsoleClient.Manager;
using NSubstitute;
using Xunit;

namespace TaskConsoleClient.UI
{
    internal class ConsoleHelper
    { 
        private readonly List<ICommandHandler> handler;

        public ConsoleHelper(List<ICommandHandler> handler)
        {
            this.handler = handler;
        }

        public void Foo(string input)
        {
            handler.First(x => x.Matches(input));
        }
    }
}
