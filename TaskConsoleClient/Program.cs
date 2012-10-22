using Ninject;
using TaskManagerClientLibrary;

namespace TaskManagerConsole
{
    public static class Program
    {
        public static void Main()
        {
            var kernel = new StandardKernel();

            kernel.Get<Application>().Run();
        }
    }
}