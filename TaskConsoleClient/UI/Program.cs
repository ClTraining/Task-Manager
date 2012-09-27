using TaskConsoleClient.Manager;
using TaskManagerHost.WCFServer;

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
