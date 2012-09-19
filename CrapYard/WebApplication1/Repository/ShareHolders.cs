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
        }

        public ShareHolders(bool isTest)
        {
            if (isTest)
            {
                Companies.Clear();
                string shareHolder = "Ahold";
                this.AddShareHolders("Ahold");
                this.AddSubsidiary("Ahold", "123");
                this.AddSubsidiary("Ahold", "ah567");

                this.AddShareHolders("Unilever");
                this.AddSubsidiary("Unilever", "123");
                this.AddSubsidiary("Unilever", "u567");

                this.AddShareHolders("Shell");
                this.AddSubsidiary("Shell", "s123");
                this.AddSubsidiary("Shell", "s567");

                this.AddShareHolders("123");
                this.AddSubsidiary("123", "c123");
                this.AddSubsidiary("123", "c123");
            }
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
            Companies.Clear();
            string shareHolder = "Ahold";
            //this.AddShareHolders("Ahold");
            //this.AddSubsidiary("Ahold", "123");
            //this.AddSubsidiary("Ahold", "ah567");

            //this.AddShareHolders("Unilever");
            //this.AddSubsidiary("Unilever", "123");
            //this.AddSubsidiary("Unilever", "u567");

            //this.AddShareHolders("Shell");
            //this.AddSubsidiary("Shell", "s123");
            //this.AddSubsidiary("Shell", "s567");

            //this.AddShareHolders("123");
            //this.AddSubsidiary("123", "c123");
            //this.AddSubsidiary("123", "c123");

            var testTree = Tree<string>.CreateTestNaryTree();
            CreateTestdictionary(testTree);
        }

        private void CreateTestdictionary(Tree<string> outerTree)
        {
            var nTree = outerTree.NTree;
            if (nTree != null)
            {
                var children = from n in nTree
                               where !string.IsNullOrEmpty(n.Data)
                               select n.Data;
                IList<string> list = null;
                if ((Companies.TryGetValue(outerTree.Data, out list)))
                    Companies.TryUpdate(outerTree.Data, children.ToList<string>(), null);
                else
                    Companies.TryAdd(outerTree.Data, children.ToList<string>());

                foreach (var tree in nTree)
                {
                    CreateTestdictionary(tree);
                }
            }
            else
            {
                IList<string> list = null;
                if (!(Companies.TryGetValue(outerTree.Data, out list)))
                    Companies.TryAdd(outerTree.Data, null);
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
            var shareHolders = Tree<string>.CreateNTree(companyPOV, Companies, Tree<string>.GetParents,
                                                        Tree<string>.TransFormXSubTreeBottomUp);

            var subsidiaries = Tree<string>.CreateNTree(companyPOV, Companies, Tree<string>.GetChildren,
                                                        Tree<string>.TransFormXSubTreeTopDown);

            IList<XElement> topDownTree;
            IList<XElement> bottomUpTree;
            XElement result = new XElement("Tree");
            Tree<string>.StackNodes.TryPop(out topDownTree);
            Tree<string>.StackNodes.TryPop(out bottomUpTree);

            var targetXElts = (bottomUpTree.First().Descendants().Where(d => d.Attribute("Text").Value == companyPOV)).ToList();
            foreach (var elts in targetXElts)
            {
                var childrenTarget = topDownTree.First().Elements();
                elts.Add(childrenTarget);
            }
            result.Add(bottomUpTree);
            return result.ToString();
        }

    }
}
