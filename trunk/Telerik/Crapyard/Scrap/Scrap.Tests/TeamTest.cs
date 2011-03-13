using System.Collections.Generic;
using DataResource;
using DataResource.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scrap.Tests
{
    
    
    /// <summary>
    ///This is a test class for TeamTest and is intended
    ///to contain all TeamTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TeamTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Repository myRepo = new Repository();
            myRepo.DeleteTeams();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Repository myRepo = new Repository();
            myRepo.DeleteTeams();
        }
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

        [TestMethod]
        public void AddTeam()
        {
            //Arrange
            Repository myRepo = new Repository();
            string name = "team1";
            string email = "test@hotmail.com";
            bool isExclusive = true;
            bool isActive = true;

            //Act
            myRepo.AddTeam(name, email, isExclusive, isActive);

            //Assert
            Team myTeam = myRepo.GetTeam(name);
            Assert.AreEqual(name, myTeam.Name);
            IList<Team> teams = myRepo.GetTeams(name);
            Assert.AreEqual(1, teams.Count);
        }
        /// <summary>
        ///A test for Team Constructor
        ///</summary>
        [TestMethod()]
        public void TeamConstructorTest()
        {
            Team target = new Team();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateTeam
        ///</summary>
        [Ignore]
        public void CreateTeamTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string email = string.Empty; // TODO: Initialize to an appropriate value
            bool isExclusive = false; // TODO: Initialize to an appropriate value
            bool isActive = false; // TODO: Initialize to an appropriate value
            Team expected = null; // TODO: Initialize to an appropriate value
            Team actual;
            actual = Team.CreateTeam(name, email, isExclusive, isActive);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Email
        ///</summary>
        [TestMethod()]
        public void EmailTest()
        {
            Team target = new Team(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Email = expected;
            actual = target.Email;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsActive
        ///</summary>
        [TestMethod()]
        public void IsActiveTest()
        {
            Team target = new Team(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.IsActive = expected;
            actual = target.IsActive;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsExclusive
        ///</summary>
        [TestMethod()]
        public void IsExclusiveTest()
        {
            Team target = new Team(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.IsExclusive = expected;
            actual = target.IsExclusive;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Team target = new Team(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
