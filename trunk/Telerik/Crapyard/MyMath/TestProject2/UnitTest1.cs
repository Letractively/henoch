using System;
using System.IO;
using System.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMath;
using MyMath.Moles;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [HostType("Moles")]
        public void Y2KCheckerTest()
        {
            //arrange
            MDateTime.NowGet = () => new DateTime(2000, 1, 1);
            //act
            FileSystem.Check();

            //assert

        }
        [TestMethod]
        public void CheckValidFile()
        {
            // arrange 
            var fileName = "test.txt";
            var content = "test";
            File.WriteAllText(fileName, content);
            
            // act 
            var test = new TestReader();
            test.LoadFile(fileName);

            // assert
            Assert.AreEqual(content, test.Content);
        }

        [TestMethod]
        public void CheckValidFile2()
        {
            // arrange 
            string fileName = "test.txt";
            var content = "test";
            var fs = new MockFileSystem();
            fs.Content = content;

            // act 
            var test = new TestReader();
            test.LoadFile(fileName);

            // assert 
            Assert.AreEqual(content, test.Content);
        }

        [TestMethod]
        public void CheckValidFileWithStubs()
        {
            // arrange 
            var fileName = "test.txt";
            var content = "test";
            //File.WriteAllText(fileName, content); 
            var fs = new SIFileSystem();
            fs.ReadAllTextString = delegate(string f)
                                       {
                                           Assert.IsTrue(f == fileName);
                                           return content;
                                       };

            // act 
            //var test = new TestReader(); 
            var test = new TestReaderWithStubs(fs);
            test.LoadFile(fileName);

            // assert 
            Assert.AreEqual(content, test.Content);
        }

        [TestMethod]
        [HostType("Moles")]
        public void CheckValidFileWithMoles()
        {
            // arrange 
            var fileName = "test.txt";
            var content = "test";
            MFileSystem.ReadAllTextString = (string f) =>
                                                {
                                                    Assert.IsTrue(f == fileName);
                                                    return content;
                                                };

            // act 
            var test = new TestReader();
            test.LoadFile(fileName);

            // assert 
            Assert.AreEqual(content, test.Content);
        }
    }
}
