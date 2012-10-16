using System;

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