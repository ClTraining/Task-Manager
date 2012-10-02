using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskConsoleClient.UI
{
    public interface ICommandHandler
    {
        bool Matches(string input);
        void Execute();
    }
}
