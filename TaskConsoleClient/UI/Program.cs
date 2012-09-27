using TaskConsoleClient.Manager;
﻿using System;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var client = new CommandManager();
            while (true)
            {
                client.Run();
            }
        }
    }
}
