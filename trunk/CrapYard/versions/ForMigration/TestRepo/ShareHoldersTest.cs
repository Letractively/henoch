using Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dictionary.BusinessObjects;

using System.Collections.Concurrent;
using Dictionary.System;


using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Dictionary.System.Caching;

namespace TestRepo
{
    
    /// <summary>
    ///This is a test class for ShareHoldersTest and is intended
    ///to contain all ShareHoldersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ShareHoldersTest
    {
        static ShareHolders _ShareHolders;
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
                    myRepository.Add(ShareHolders.ShareHolderLabel, value);
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
                dictionary = myRepository.GetData(ShareHolders.ShareHolderLabel) as ConcurrentDictionary<string, IList<string>>;
                if (dictionary == null)
                {
                    //Initialize
                    dictionary = new ConcurrentDictionary<string, IList<string>>();
                    myRepository.Add(ShareHolders.ShareHolderLabel, dictionary);
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _ShareHolders = new ShareHolders();
            _ShareHolders.InsertCorporate(CreateTestCorporate());
            _ShareHolders.InsertCorporate(CreateTestCorporate2());

            string xmlString = @"
                                    
                      <node Text='Root1'>
                        
                       
                      </node>   
                    ";

            _ShareHolders.InsertCorporate(CreateTestCorporate3(xmlString));
            string xmlString2 = @"
                                    
                      <node Text='Root2'>
                        
                       
                      </node>   
                    ";

            _ShareHolders.InsertCorporate(CreateTestCorporate3(xmlString2));
        }
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
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            Assert.AreEqual(1, _ShareHolders.GetSubsidiaries("S211").Count);
            var children = ShareHolders.Companies["VirtualRoot"];
            Assert.AreEqual(children.Count, _ShareHolders.GetSubsidiaries("VirtualRoot").Count,
                string.Format("VR should have (0) children.", children.Count));

            Assert.AreEqual(2,_ShareHolders.GetShareHolders("S211").Count);

            IList<string> childWithMoreParents = ShareHolders.Companies.
                Where(subsidiary => _ShareHolders.GetShareHolders(subsidiary.Key).Count > 1).
                Select( c => c.Key).ToList();
            Assert.AreEqual(4, childWithMoreParents.Count());
        }
        [TestMethod]
        public void OrganoTreeOverviewTest()
        {
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            string xml = _ShareHolders.CreateXMLOrganoTreeView("root", RelationView.Overview);
            XElement xTree = XElement.Parse(xml);
            Assert.AreEqual(21, xTree.Descendants().Count());

            xml = _ShareHolders.CreateXMLOrganoTreeView("company does not exist", RelationView.Overview);
            xTree = XElement.Parse(xml);
            Assert.AreEqual(0, xTree.Descendants().Count());
        }
        /// <summary>
        /// Hide Virtual root
        /// </summary>
        [TestMethod]
        public void OrganoTreeDependenciesTest()
        {
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            string xml = _ShareHolders.CreateXMLOrganoTreeView("S211", RelationView.Dependencies);
            XElement xTree = XElement.Parse(xml);
            Assert.AreEqual(5, xTree.Descendants().Count());
            Console.WriteLine(xml);
        }
        [TestMethod]
        public void GetCykelTest()
        {
            //create cykel
            _ShareHolders.AddSubsidiary("S11", "root");
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            string xml = _ShareHolders.CreateXMLOrganoTreeView("root", RelationView.Overview);
            XElement xTree = XElement.Parse(xml);
            Assert.AreEqual(22, xTree.Descendants().Count());
            Console.WriteLine(xml);
           
            bool isInCycle = new Tree<string>().IsInCycle("S211", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(false, isInCycle, "S211 should be in cycle (0).");

            _ShareHolders.CreateXMLOrganoTreeView("S11", RelationView.Overview);            
            isInCycle = new Tree<string>().IsInCycle("S11", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(true, isInCycle, "S11 should be in cycle (0).");

            isInCycle = new Tree<string>().IsInCycle("root", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(true, isInCycle, "root should be in cycle (0).");

            //undo cycle
            _ShareHolders.RemoveSubsidiary("S11", "root");
            xTree = _ShareHolders.CreateXMLCorporate("S211", "root", RelationView.Overview);            

            isInCycle = new Tree<string>().IsInCycle("S11", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(false, isInCycle, "S11 should not be in cycle.");
            isInCycle = new Tree<string>().IsInCycle("root", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(false, isInCycle, "root should not be in cycle.");

            //create cykel
            _ShareHolders.AddSubsidiary("S11", "root");
            isInCycle = new Tree<string>().IsInCycle("S11", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(true, isInCycle, "S11 should not be in cycle.");
            isInCycle = new Tree<string>().IsInCycle("root", ShareHolders.ShareHolderLabel);
            Assert.AreEqual(true, isInCycle, "root should not be in cycle.");
        }
        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void CykelTest()
        {
            //create cykel
            _ShareHolders.AddSubsidiary("S11", "root");
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            string xml = _ShareHolders.CreateXMLOrganoTreeView("S211", RelationView.Overview);
            XElement xTree = XElement.Parse(xml);
            Assert.AreEqual(44, xTree.Descendants().Count());
            Console.WriteLine(xml);

        }
        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void Cykel2Test()
        {
            //create cykel
            _ShareHolders.AddSubsidiary("Root2", "Root1");
            _ShareHolders.AddSubsidiary("Root1", "Root2");
            Assert.AreEqual(29, ShareHolders.Companies.Count());
            string xml = _ShareHolders.CreateXMLOrganoTreeView("Root1", RelationView.Overview);
            Console.WriteLine(xml);
            XElement xTree = XElement.Parse(xml);
            Assert.AreEqual("Root1", xTree.Elements("Node").First().Attribute("Text").Value);
        }
        /// <summary>
        /// Only for testing. A dictionary will be extended with xml
        /// </summary>
        /// <param name="result"></param>
        private static XElement CreateTestCorporate()
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

            return xml;

        }
        private static XElement CreateTestCorporate2()
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(@"
<Node CssClass='defaultNode' Text='root' Expanded='True'>
    <Node CssClass='defaultNode' Text='S11' Expanded='True'>
        <Node CssClass='found' Text='S211' Expanded='False'>
            <Node CssClass='defaultNode' Text='S22' Expanded='True' />
        </Node>
        <Node CssClass='found' Text='S221' Expanded='False'>
            <Node CssClass='defaultNode' Text='m1' Expanded='True' />
            <Node CssClass='defaultNode' Text='S21' Expanded='True' />
            <Node CssClass='defaultNode' Text='Sp' Expanded='True' />
        </Node>
    </Node>
    <Node CssClass='defaultNode' Text='S211' Expanded='True'>
        <Node CssClass='defaultNode' Text='S22' Expanded='True'>
            <Node CssClass='found' Text='S41' Expanded='False'>
                <Node CssClass='defaultNode' Text='S51' Expanded='True' />
                <Node CssClass='defaultNode' Text='S52' Expanded='True' />
            </Node>
            <Node CssClass='defaultNode' Text='S42' Expanded='True' />
        </Node>
    </Node>
    <Node CssClass='defaultNode' Text='S221' Expanded='True'>
        <Node CssClass='defaultNode' Text='m1' Expanded='True' />
        <Node CssClass='defaultNode' Text='S21' Expanded='True' />
        <Node CssClass='defaultNode' Text='Sp' Expanded='True' />
    </Node>
    <Node CssClass='defaultNode' Text='S41' Expanded='True'>
        <Node CssClass='defaultNode' Text='S51' Expanded='True' />
        <Node CssClass='defaultNode' Text='S52' Expanded='True' />
    </Node>
</Node>
                    ");

            return xml;

        }
        private static XElement CreateTestCorporate3(string xmlString)
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(xmlString);

            return xml;

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
