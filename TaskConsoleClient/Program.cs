using Ninject;

namespace TaskManagerConsole
{
    public static class Program
    {
        public static void Main()
        {
            var kernel = new StandardKernel();

            kernel.Get<TaskManagerClientLibrary.Application>().Run();
        }
    }
}