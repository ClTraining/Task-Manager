﻿using TaskConsoleClient.Manager;
﻿using System;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var helper = new ConsoleHelper(new CommandManager(new NetTcpConnection()));
            while (true)
            {
                helper.Parse(Console.ReadLine());
            }
        }
    }
}
