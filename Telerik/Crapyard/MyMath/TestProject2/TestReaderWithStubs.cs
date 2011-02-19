using System;
using MyMath;

namespace TestProject2
{
    public class TestReaderWithStubs
    {
        private IFileSystem fs;
        //constructor 
        public TestReaderWithStubs(IFileSystem fs)
        {
            this.fs = fs;
        }

        public void LoadFile(string fileName)
        {
            var content = this.fs.ReadAllText(fileName);
            if (!content.StartsWith("test")) throw new ArgumentException("invalid file");
            this.Content = content;
        }

        public string Content { get; private set; }
    }
}