using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectToWcf;

namespace TaskManagerClientLibrary.ConcreteHandlers
{
    class Help : Command<string>
    {
        public Help(Type derived) : base(typeof(Help)) { }

        protected override void ExecuteWithGenericInput(string input)
        {
            Console.WriteLine("Hello world!");
        }
    }
}
