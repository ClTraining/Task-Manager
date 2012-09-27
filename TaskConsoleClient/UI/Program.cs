﻿using TaskConsoleClient.Manager;
﻿using System;

namespace TaskConsoleClient.UI
{
    static class Program
    {
        static void Main()
        {
            var cm = new CommandManager();
            while (true)
            {
                cm.Run();
            }
        }
    }
}
