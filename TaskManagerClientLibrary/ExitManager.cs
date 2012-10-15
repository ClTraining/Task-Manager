using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClientLibrary
{
    public class ExitManager
    {
        public virtual void Close()
        {
            Environment.Exit(0);
        }
    }
}
