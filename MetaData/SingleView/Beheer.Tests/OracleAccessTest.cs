using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using OraAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.DataAccess.Client;


namespace MetaData.Beheer.Tests
{
    
    
    /// <summary>
    ///This is a test class for OracleAccessTest and is intended
    ///to contain all OracleAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OracleAccessTest
    {


        private TestContext testContextInstance;
        private string _oradb = "Data Source=odomeinen;User Id=so_dom;Password=supermx;";

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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetOraConnection
        ///</summary>
        [TestMethod]
        public void GetOraConnectionTest()
        {            
            var target = new OracleAccess();
            OracleConnection expected ;
            using (expected = new OracleConnection(_oradb))
            {
                expected.Open();
                new OracleCommand { Connection = expected };
            }
            OracleConnection actual;
            actual = target.GetOraConnection();
            Assert.AreEqual(expected.InstanceName, actual.InstanceName);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod]
        public void GetThemasTest()
        {
            var target = new OracleAccess();
            bool actual;
            actual = target.SetOraConnection();

            Assert.AreEqual(true, actual);
            IList<BeheerContextEntity> list = target.GetBusinessObjects("thema", "themanaam");
            Assert.IsTrue(list.Count>0);

        }
        [TestMethod]
        public void InsertThemasTest()
        {
            var target = new OracleAccess();        

            var themaNew = new BeheerContextEntity
            {
                DataKeyValue = "aaaDwight" + DateTime.Now,
                Tablename = "thema",
                DataKeyName = "themanaam"
            };
            int rowsAffected = target.Insert(themaNew);
            var expected = target.GetBusinessObject(themaNew.Tablename, themaNew.DataKeyName, 
                themaNew.DataKeyValue) as BeheerContextEntity;

            Assert.IsTrue(rowsAffected == 1);
            // ReSharper disable PossibleNullReferenceException
            Assert.AreEqual(expected.DataKeyValue, themaNew.DataKeyValue);
            // ReSharper restore PossibleNullReferenceException
            Assert.AreEqual(expected.Tablename, themaNew.Tablename);
            Assert.AreEqual(expected.DataKeyName, themaNew.DataKeyName);

        }
        [TestMethod]
        public void UpdateThemasTest()
        {
            var target = new OracleAccess();            

            string nu = DateTime.Now.ToString();
            var themaOld = new BeheerContextEntity
            {
                DataKeyValue = "aaP" + nu,
                Tablename = "thema",
                DataKeyName = "themanaam"
            };
            target.Insert(themaOld);

            var themaNew= new BeheerContextEntity
                                              {
                                                  DataKeyValue = "aapje" + nu,
                                                  Tablename = "thema",
                                                  DataKeyName = "themanaam"
                                              }; 
            int rowsAffected  = target.Update(themaOld,themaNew);
            Assert.AreEqual(1,rowsAffected );

            var expected = target.GetBusinessObject(themaNew.Tablename, themaNew.DataKeyName,
                themaNew.DataKeyValue) as BeheerContextEntity;
            // ReSharper disable PossibleNullReferenceException
            Assert.AreEqual(expected.DataKeyValue, themaNew.DataKeyValue);
            // ReSharper restore PossibleNullReferenceException
            Assert.AreEqual(expected.Tablename, themaNew.Tablename);
            Assert.AreEqual(expected.DataKeyName, themaNew.DataKeyName);
        }

        [TestMethod]
        public void DeleteThemasTest()
        {
            var target = new OracleAccess();

            var thema = new BeheerContextEntity
            {
                DataKeyValue = "aaaDwight" + DateTime.Now,
                Tablename = "thema",
                DataKeyName = "themanaam"
            };

            Assert.IsTrue(target.Insert(thema) > 0);

            int rowsAffected = target.Delete(thema);

            Assert.IsTrue(rowsAffected == 1);
            var expected = target.GetBusinessObject(thema.Tablename, thema.DataKeyName,
                thema.DataKeyValue) as BeheerContextEntity;
            Assert.AreEqual(expected, null);

        }
    }
}
