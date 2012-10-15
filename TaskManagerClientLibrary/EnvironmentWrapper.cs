using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClientLibrary
{
    public class EnvironmentWrapper
    {
        public virtual void Exit()
        {
            Environment.Exit(0);
        }
    }
}
