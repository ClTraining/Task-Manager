using System;
using System.IO;
using TaskConsoleClient.Manager;
using TaskManagerHost.WCFServer;

namespace TaskConsoleClient.UI
{
    static class Program
    {

        static void Main()
        {
            var helper = new ConsoleHelper(new CommandManager());
            string s;
            while ((s = Console.ReadLine()) != null)
            {
                helper.Parse(s);
            }
        }
    }
}
