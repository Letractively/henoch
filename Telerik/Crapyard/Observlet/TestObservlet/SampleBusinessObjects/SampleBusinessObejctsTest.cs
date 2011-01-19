using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Rhino.Mocks;
using Assert=NUnit.Framework.Assert;

namespace TestObservlet.SampleBusinessObjects
{
    /// <summary>
    /// Summary description for SampleBusinessObejctsTest
    /// </summary>
    [TestClass]
    [TestFixture]
    public class SampleBusinessObejctsTest
    {
        public SampleBusinessObejctsTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [Test]
        public void MockAGenericInterface()
        {
            MockRepository mocks = new MockRepository();
            IList<int> list = mocks.CreateMock<IList<int>>();
            Assert.IsNotNull(list);
            Expect.Call(list.Count).Return(5);
            mocks.ReplayAll();

            Assert.AreEqual(5, list.Count);

            mocks.VerifyAll();
        }
        [Test]
        public void CreateAnimalStub_MockRepositoryStub()
        {
            MockRepository mocks = new MockRepository();
            IAnimal animal = mocks.Stub<IAnimal>(); 
            animal.Name = "Snoopy"; 
            Assert.AreEqual("Snoopy", animal.Name);
        }

    }
}