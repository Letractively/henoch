using MyMath;

namespace TestProject2
{
    public class MockFileSystem : IFileSystem
    {
        public string Content;

        public string ReadAllText(string fileName)
        {
            return this.Content;
        }
    }
}