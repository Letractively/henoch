using Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dictionary.BusinessObjects;

using System.Collections.Concurrent;
using Dictionary.System;

using Caching;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TestRepo
{
    
    /// <summary>
    ///This is a test class for ShareHoldersTest and is intended
    ///to contain all ShareHoldersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ShareHoldersTest
    {

        private const string cShareHolder = "testcompanies";
        private TestContext testContextInstance;
        public static ConcurrentDictionary<string, IList<string>> Companies
        {
            get
            {
                ConcurrentDictionary<string, IList<string>> list = InitializeCache();

                return list;
            }
            private set
            {
                var myRepository = MyCache<Object>.CacheManager;
                if (myRepository != null)
                {
                    myRepository.Add(cShareHolder, value);
                }
            }
        }
        /// <summary>
        /// Returns the dictionary from cache.
        /// See http://xlinux.nist.gov/dads//HTML/dictionary.html
        /// </summary>
        /// <returns></returns>
        private static ConcurrentDictionary<string, IList<string>> InitializeCache()
        {
            ConcurrentDictionary<string, IList<string>> dictionary = null;
            var myRepository = MyCache<Object>.CacheManager;
            if (myRepository != null)
            {
                dictionary = myRepository.GetData(cShareHolder) as ConcurrentDictionary<string, IList<string>>;
                if (dictionary == null)
                {
                    //Initialize
                    dictionary = new ConcurrentDictionary<string, IList<string>>();
                    myRepository.Add(cShareHolder, dictionary);
                }
            }
            return dictionary;
        }
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
        ///A test for counting ShareHolders of a subsidiary
        ///</summary>
        [TestMethod]
        public void CheckShareHoldersTest()
        {
            ShareHolders shareHolders = new ShareHolders(true);
            
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            Assert.AreEqual(1, shareHolders.GetSubsidiaries("S211").Count);
            Assert.AreEqual(4, shareHolders.GetSubsidiaries("VirtualRoot").Count);

            Assert.AreEqual(2,shareHolders.GetShareHolders("S211").Count);
                       
            var childWithMoreParents = ShareHolders.Companies.Where(subsidiary => shareHolders.GetShareHolders(subsidiary.Key).Count > 1);
            Assert.AreEqual(4, childWithMoreParents.Count());
        }

        /// <summary>
        /// Only for testing. A dictionary will be extended with xml
        /// </summary>
        /// <param name="result"></param>
        private void CreateTestCorporate()
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(@"
                                    
                      <node Text='Stern Groep N.V.'>
                        
                        <node Text='Stern Beheer B.V.'>
                            
                            <node Text='SternDealers N.V.'>
                                 <node Text='Stern 1 B.V.'>
                                    <node Text='Stern Auto B.V.'>
                                    </node> 
                                 </node> 
                                 <node Text='Stern 2 B.V.'>
                                 </node> 
                                 <node Text='Stern 3 B.V.'>
                                    <node Text='Ardea Auto B.V.'>
                                        <node Text='Ardea Rotterdam-Noord'>
                                        </node> 
                                    </node> 
                                 </node> 
                                 <node Text='Stern 4 B.V.'>
                                    <node Text='Arend Auto'>
                                    </node> 
                                    <node Text='S42'>
                                    </node> 
                                 </node> 
                            </node> 
                   
                            <node Text='SternUniverseel'>
                                 <node Text='Stern Occassions'>
                                    <node Text='Sternplaza B.V.'>
                                    </node> 
                                 </node> 
                            </node> 

                        </node>   

                      </node>   
                    ");

            InsertTestCorporateIntoDictionary(xml);

        }
        private void CreateTestCorporate2()
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(@"
                                    
                      <node Text='Root1'>
                        
                       
                      </node>   
                    ");

            InsertTestCorporateIntoDictionary(xml);

        }
        private void CreateTestCorporate3()
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(@"
                                    
                      <node Text='Root2'>
                        
                       
                      </node>   
                    ");

            InsertTestCorporateIntoDictionary(xml);

        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        private void InsertTestCorporateIntoDictionary(XElement xml)
        {
            var testTree = Tree<string>.CreateParseTree(xml, CreateXElts);
            ConcurrentDictionary<string, IList<string>> testCompanies = new ConcurrentDictionary<string, IList<string>>();
            CreateTestdictionary(testTree, Companies);

        }

        private void CreateXElts(XElement newTestTree)
        {
            //dummy;
        }
        private void CreateTestdictionary(Tree<string> outerTree, ConcurrentDictionary<string, IList<string>> dictionary)
        {
            IList<Tree<string>> nTree = outerTree.NTree;
            if (nTree != null)
            {
                var children = from n in nTree
                               where !string.IsNullOrEmpty(n.Data)
                               select n.Data;
                IList<string> list = null;
                if ((dictionary.TryGetValue(outerTree.Data, out list)))
                    dictionary.TryUpdate(outerTree.Data, children.ToList<string>(), list);
                else
                    dictionary.TryAdd(outerTree.Data, children.ToList<string>());

                foreach (var tree in nTree)
                {
                    CreateTestdictionary(tree, dictionary);
                }
            }
            else
            {
                IList<string> list = null;
                if (!(dictionary.TryGetValue(outerTree.Data, out list)))
                    dictionary.TryAdd(outerTree.Data, new List<string>());
            }

        }
    }
}
