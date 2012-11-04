using System.IO;

namespace TaskManagerServiceLibrary.Repositories
{
    public class FileOperationsWrapper
    {
        private string path;

        public FileOperationsWrapper() { }
        public FileOperationsWrapper(string path)
        {
            this.path = path;
        }
        public virtual void SaveToFile(string info)
        {
            var stream = new FileStream(path, FileMode.Create);
            var writer = new StreamWriter(stream);
            writer.Write(info);
            writer.Close();
            stream.Close();

        }

        public virtual string GetInfo()
        {
            string text;
            try
            {
                text = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                text = "";
            }
            return text;
        }
    }
}
