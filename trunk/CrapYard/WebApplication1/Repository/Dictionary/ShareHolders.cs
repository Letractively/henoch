using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using ParallelResourcer;
using System.Collections.Concurrent;
using System.Xml.Linq;
using System.ComponentModel;
using Dictionary.System;

namespace Dictionary.BusinessObjects
{
    public enum RelationView
    {
        Overview = 0,
        Dependencies = 1
    }
    /// <summary>
    /// Focusses on shareholders and their relations. The data is cached.
    /// </summary>
    [Serializable]
    [DataObjectAttribute]
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

            AddOrUpdateVirtualRoot();
        }
        /// <summary>
        /// Introduce a Virtual Root (NOT to be shown in treeView) to identify a maximum cycle
        /// </summary>
        public void AddOrUpdateVirtualRoot()
        {
            #region introduce Virtual Root (NOT to be shown in treeView) to identify a maximum cycle
            IList<string> listVR = null;
            Companies.TryRemove(VirtualRoot, out listVR);

            var roots = Companies.Where(c => GetShareHolders(c.Key).Count == 0 ).Select(c => c.Key);
            if (Companies.Count() > 0 && roots.Count() > 0)
            {
                if (!Companies.TryAdd(VirtualRoot, roots.ToList()))
                {
                    var oldList = Companies[VirtualRoot];
                    Companies.TryUpdate(VirtualRoot, roots.ToList(), oldList);
                };
            }
            #endregion
        }

        public ShareHolders(bool isTest)
        {
            if (isTest)
            {
                CreateTestData();
            }
            AddOrUpdateVirtualRoot();
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
            CreateTestCorporate();// tree 1
            CreateTestCorporate2();// tree 2
            CreateTestCorporate3();// tree 3
            #endregion

            var testTree = Tree<string>.CreateTestNaryTree();// tree 4
            AddOrUpdateDictionary(testTree, Companies);

        }
        [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
        public IList<BeheerContextEntity> GetCompanies()
        {
            IList<string> list = Companies.Select(c => c.Key).OrderBy( e => e.ToString()).ToList();
            IList<BeheerContextEntity> res = new List<BeheerContextEntity>();
            foreach (var item in list)
            {
                BeheerContextEntity entity = new BeheerContextEntity()
                {
                    DataKeyValue = item
                };
                res.Add(entity);
            }
            return res;
        }
        /// <summary>
        /// linked list of parents and their children. Companies are stored in cache. 
        /// </summary>
        /// <returns></returns>
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
        /// Gets relations from cache
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static Tree<string> GetRelations (string root)
        {
            Tree<string> tree = null;

            var myRepository = MyCache<object>.CacheManager;
            if (myRepository != null)
            {
                tree = myRepository.GetData(root) as Tree<string>;
            }

            return tree;
        }

        public static void SetRelations(string rootKey, Tree<string> treeValue)
        {
            var myRepository = MyCache<object>.CacheManager;
            if (myRepository != null)
            {
                myRepository.Add(rootKey, treeValue);
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
        /// <summary>
        /// Adds subsidiary under the contraints of the rules
        /// </summary>
        /// <param name="shareHolder"></param>
        /// <param name="subsidiary"></param>
        /// <returns></returns>
        public bool AddSubsidiary(string shareHolder, string subsidiary)
        {
            bool validation = true;

            try
            {
                ///business rules
                if (!ValidateAPriori(shareHolder,subsidiary))
                    return false;
                var candidates = Companies[shareHolder].Where(subs => subs.Equals(subsidiary));
                ///No duplicates are allowed.
                if (candidates.Count() > 0)
                    return false;

                Companies[shareHolder].Add(subsidiary);

                //remove from virtual root if needed
                RemoveSubsidiary(ShareHolders.VirtualRoot, subsidiary);
            }
            catch
            {
                validation = false;
            }

            return validation;
        }
        /// <summary>
        /// Virtual root cannot be changed by the viewer.
        /// 
        /// </summary>
        /// <param name="shareHolder"></param>
        /// <param name="subsidiary"></param>
        /// <returns></returns>
        public static bool ValidateAPriori(string shareHolder, string subsidiary)
        {
            //business rule
            if (shareHolder.Equals(VirtualRoot))
                return false;
            if (subsidiary.Equals(VirtualRoot))
                return false;
            if (subsidiary.Equals(shareHolder))
                return false;
            return true;
        }
        /// <summary>
        /// A root has only 1 parent : its virtual root.
        /// </summary>
        /// <param name="shareHolder"></param>
        /// <param name="subsidiary"></param>
        public void RemoveSubsidiary(string shareHolder, string subsidiary)
        {
            if (shareHolder.Equals(VirtualRoot))
            {
                //clean up the virtual root: the virtual root should have only roots as children.
                var roots = GetSubsidiaries(VirtualRoot);
                var commonRoots = Tree<string>.GetParents(subsidiary, Companies);
                if(commonRoots.Count() > 1) //then subsidiary is not a root!
                    Companies[VirtualRoot].Remove(subsidiary);
            }
            else
            {
                bool succeeded = Companies[shareHolder].Remove(subsidiary);
                if (succeeded && !shareHolder.Equals(VirtualRoot))
                {
                    var roots = GetSubsidiaries(VirtualRoot);
                    bool isRootAlready = roots.Where(r => r.Equals(subsidiary)).Count() > 0;
                    if (!isRootAlready)
                        Companies[VirtualRoot].Add(subsidiary);
                }
            }
            
        }
        /// <summary>
        /// Reloads cache from dataresources.
        /// </summary>
        public void Refresh()
        {
            CreateTestData();
            AddOrUpdateVirtualRoot();

            //TODO: make refresh
        }

        private void AddOrUpdateDictionary(Tree<string> outerTree,  ConcurrentDictionary<string,IList<string>> dictionary)
        {
            var nTree = outerTree.NTree;
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
                    AddOrUpdateDictionary(tree,  dictionary);
                }
            }
            else
            {
                IList<string> list = null;
                if (!(dictionary.TryGetValue(outerTree.Data, out list)))
                    dictionary.TryAdd(outerTree.Data, new List<string>());
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
        public string CreateXMLOrganoTreeView(string companyPOV, RelationView view)
        {

            XElement result = new XElement("Tree");

            try
            {
                IList<string> subsidiariesPOV;
                string parsedSearchItem = ParseSearchString(companyPOV);
                //TODO: ParseSearchString
                if (Companies.TryGetValue(companyPOV, out subsidiariesPOV))
                {
                    XElement xTree;
                    switch (view)
                    {
                        case RelationView.Overview:
                            IList<string> roots = new Tree<string>().GetRoots(VirtualRoot, companyPOV, Companies);

                            if (roots.Count == 0)
                            {
                                xTree = CreateXMLCorporate(companyPOV, companyPOV, view);
                                result.Add(xTree);
                            }
                            else
                                foreach (var root in roots)
                                {
                                    xTree = CreateXMLCorporate(companyPOV, root, view);

                                    result.Add(xTree);
                                }

                            break;
                        case RelationView.Dependencies:
                            xTree = CreateXMLCorporate(companyPOV, "n.a.", view);
                            //hide virtual root 
                            var children = xTree.Elements();
                            result.Add(children);
                            break;
                        default:
                            throw new NotImplementedException("Relation not defined yet.");
                            break;
                    }

                }
            }
            catch (Exception ex)
            {                
                throw new ApplicationException(ex.Message);
            }
            return result.ToString();
        }

        private XElement CreateXMLCorporate(string companyPOV, string root,RelationView view)
        {
            XElement xTree;
            try
            {
                IList<XElement> outerTrack = new Tree<string>().CreateXMLOuterTrack(root);

                Tree<string> tree = null;

                switch (view)
                {
                    case RelationView.Overview:
                        tree = new Tree<string>().CreateNTree(outerTrack, root, Companies, Tree<string>.GetChildren,
                                                new Tree<string>().TransFormXSubTreeTopDown,
                                                Tree<string>.CreateXmlElementsTopDown);
                        break;
                    case RelationView.Dependencies:
                        tree = new Tree<string>().CreateNTree(outerTrack, companyPOV, Companies, Tree<string>.GetParents,
                                                        new Tree<string>().TransFormXSubTreeBottomUp,
                                                        Tree<string>.CreateXmlElementsBottomUp);
                        break;
                    default:
                        break;
                }

                SetRelations(root, tree);

                IList<XElement> topDownTree;

                tree.StackNodes.TryPop(out topDownTree);

                xTree = topDownTree.First();
                //ColorFoundNodes(companyPOV, xTree);

                tree.StackNodes.Push(topDownTree);
                SetRelations(root, tree);
            }
            catch (Exception ex)
            {
                
                throw;
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
            
            InsertCorporate(xml);

        }
        private void CreateTestCorporate2()
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(@"
                                    
                      <node Text='Root1'>
                        
                       
                      </node>   
                    ");

            InsertCorporate(xml);

        }
        private void CreateTestCorporate3()
        {
            //NOTE: 2 roots are possible, result.Add(newTesttree);
            XElement xml = XElement.Parse(@"
                                    
                      <node Text='Root2'>
                        
                       
                      </node>   
                    ");

            InsertCorporate(xml);

        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        public void InsertCorporate(XElement xml)
        {
            var corporateTree = Tree<string>.CreateParseTree(xml, CreateXElts);
            ConcurrentDictionary<string, IList<string>> testCompanies = new ConcurrentDictionary<string, IList<string>>();
            AddOrUpdateDictionary(corporateTree, Companies);

            AddOrUpdateVirtualRoot();
        }

        private void CreateXElts(XElement newTestTree)
        {
            //dummy;
        }


    }
}
