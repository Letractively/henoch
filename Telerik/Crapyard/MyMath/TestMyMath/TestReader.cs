using System;

namespace MyMath
{
    public class TestReader
    {
        private IFileSystem fs;

        public string Content { get; private set; }

        internal void LoadFile(string fileName)
        {
            var content = FileSystem.ReadAllText(fileName);
            if (!content.StartsWith("test")) throw new ArgumentException("invalid file");
            this.Content = content;
        }
    }
}