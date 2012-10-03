using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using ParallelResourcer;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace Repository
{
    /// <summary>
    /// Focusses on shareholders and their relations. The data is cached.
    /// </summary>
    public class ShareHolders
    {
        private const string cShareHolder ="shareholders";
        public static string VirtualRoot 
        { get
            {
                return "VirtualRoot";
            }
        }
        /// <summary>
        /// linked list of parents and their children
        /// </summary>
        IDictionary<string, string> _organiGraph = new Dictionary<string, string>();
        /// <summary>
        /// load the cache from dataresources involved
        /// </summary>
        public ShareHolders()
        {
            InitializeCache();

            CreateVirtualRoot();
        }

        private void CreateVirtualRoot()
        {
            #region introduce Virtual Root (NOT to be showed in treeView) to prevent a maximum cycle
            var roots = Companies.Where(c => GetShareHolders(c.Key).Count == 0).Select(c => c.Key);
            if (Companies.Count()>0 && Companies.Where(c => c.Key.Equals(VirtualRoot)).Count() == 0)
            {
                Companies.TryAdd(VirtualRoot, roots.ToList());
            }
            #endregion
        }

        public ShareHolders(bool isTest)
        {
            if (isTest)
            {
                CreateTestData();
            }
        }

        private void CreateTestData()
        {

            //var myRepository = MyCache<Object>.CacheManager;
            //if (myRepository != null)
            //{
            //    var dictionary = myRepository.GetData(cShareHolder) as ConcurrentDictionary<string, IList<string>>;
            //    if (dictionary != null)
            //    {
            //        //Initialize
            //        dictionary = new ConcurrentDictionary<string, IList<string>>();
            //        myRepository.Add(cShareHolder, dictionary);
            //    }
            //}

            #region create test corporates
            CreateTestCorporate();
            CreateTestCorporate2();
            CreateTestCorporate3();
            #endregion

            var testTree = Tree<string>.CreateTestNaryTree();
            CreateTestdictionary(testTree, Companies);

        }
        /// <summary>
        /// linked list of parents and their children. Companies are stored in cache. 
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, IList<string>> Companies
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
        public IList<string> GetSubsidiaries(string shareHolder)
        {
            IList<string> list =Tree<string>.GetChildren(shareHolder, Companies);
            return list;

        }
        public IList<string> GetShareHolders(string subsidiary)
        {
            var listShareHolders =Tree<string>.GetParents(subsidiary, Companies);

            return listShareHolders.ToList<string>();
        }

        /// <summary>
        /// adds a new shareholder with empty list of subsidiaries.
        /// </summary>
        /// <param name="shareHolder"></param>
        public void AddShareHolders(string shareHolder)
        {
            Companies.TryAdd(shareHolder, new List<string>());
        }
        public void AddSubsidiary(string shareHolder, string subsidiary)
        {
            Companies[shareHolder].Add(subsidiary);
        }
        public void RemoveSubsidiary(string shareHolder, string subsidiary)
        {
            Companies[shareHolder].Remove(subsidiary);

        }
        /// <summary>
        /// Reloads cache from dataresources.
        /// </summary>
        public void Refresh()
        {
            CreateTestData();
            CreateVirtualRoot();

            //TODO: make refresh
        }

        private void CreateTestdictionary(Tree<string> outerTree,  ConcurrentDictionary<string,IList<string>> dictionary)
        {
            var nTree = outerTree.NTree;
            if (nTree != null)
            {
                var children = from n in nTree
                               where !string.IsNullOrEmpty(n.Data)
                               select n.Data;
                IList<string> list = null;
                if ((dictionary.TryGetValue(outerTree.Data, out list)))
                    dictionary.TryUpdate(outerTree.Data, children.ToList<string>(), null);
                else
                    dictionary.TryAdd(outerTree.Data, children.ToList<string>());

                foreach (var tree in nTree)
                {
                    CreateTestdictionary(tree,  dictionary);
                }
            }
            else
            {
                IList<string> list = null;
                if (!(dictionary.TryGetValue(outerTree.Data, out list)))
                    dictionary.TryAdd(outerTree.Data, null);
            }

        }
        public Tree<string> GetRoot(string company)
        {
           Tree<string> NTree = new Tree<string>();

            

            
            return NTree;
        }

        private void GetAncestors(string candidate)
        {
            IList<string> listAncestors = GetShareHolders(candidate);
            foreach (var item in listAncestors)
            {
                
            }

            if (listAncestors != null && listAncestors[0].Equals(candidate))
            {
                listAncestors.Add(candidate);
            }
            else
            {

                listAncestors.Add(candidate);
            }

            ;
        }
        /// <summary>
        /// Creates an XML string representing an Organogram-ish: organotree. 
        /// </summary>
        /// <param name="companyPOV">The company's point of view in the organisation.</param>
        /// <returns></returns>
        public string CreateXMLOrganoTreeView(string companyPOV)
        {

            XElement result = new XElement("Tree");

            IList<string> subsidiariesPOV;
            string parsedSearchItem = ParseSearchString(companyPOV);
            //TODO: ParseSearchString
            if (Companies.TryGetValue(companyPOV, out subsidiariesPOV))
            {
                
                IList<string> roots = new Tree<string>().GetRoots(VirtualRoot, companyPOV, Companies);

                foreach (var root in roots)
                {
                    XElement xTree = CreateXMLCorporate(companyPOV, root);

                    result.Add(xTree);
                }

            }
            return result.ToString();
        }

        private XElement CreateXMLCorporate(string companyPOV, string root)
        {

            IList<XElement> outerTrack = new List<XElement>() 
                {  
                    new XElement("Node",
                            new XAttribute("Text","TrackRoot" + Guid.NewGuid().ToString()),
                            new XAttribute("Expanded", "True"),
                            new XAttribute("CssClass", "defaultNode"),
                            new XElement("Node",
                                new XAttribute("Text", root),
                                new XAttribute("CssClass", "defaultNode"),
                                new XAttribute("Expanded", "True")))
                };
            new Tree<string>().CreateNTree(outerTrack, root, Companies, Tree<string>.GetChildren,
                                                        Tree<string>.TransFormXSubTreeTopDown);

            IList<XElement> topDownTree;

            Tree<string>.StackNodes.TryPop(out topDownTree);

            XElement xTree = topDownTree.First();
            var foundList = (xTree.Descendants().Where(d => d.Attribute("Text").Value == companyPOV)).ToList();

            for (int i = 0; i < foundList.Count; i++)
            {
                XElement newNode = new XElement("Node",
                        new XAttribute("Text", companyPOV),
                        new XAttribute("CssClass", foundList[i].Attribute("CssClass").Value),
                        new XAttribute("Expanded", foundList[i].Attribute("Expanded").Value),
                        new XAttribute("BackColor", "Gold"));
                newNode.Add(foundList[i].Elements());
                foundList[i].ReplaceWith(newNode);
            }
            return xTree;
        }

        private string ParseSearchString(string companyPOV)
        {
            return String.Empty;//
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


    }
}
