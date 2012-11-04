using System.IO;
using FluentAssertions;
using Xunit;

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

    public class FileOperationsWrapperTests
    {
        FileOperationsWrapper wrapper=new FileOperationsWrapper("someSave.txt");
        FileOperationsWrapper wrapper2 = new FileOperationsWrapper("someSave2.txt");
        
        [Fact]
        public void should_return_empty_string_if_file_contains_nothing()
        {
            var info = wrapper2.GetInfo();

            info.Should().BeEmpty();
        }

        [Fact]
        public void should_save_argument_string_to_file()
        {
            wrapper.SaveToFile("Hello world");

            File.ReadAllText("someSave.txt").Should().NotBeEmpty();
        }

    }
}
