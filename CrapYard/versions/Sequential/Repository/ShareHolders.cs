using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using ParallelResourcer;

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

        /// <summary>
        /// linked list of parents and their children. Companies are stored in cache. 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IList<string>> Companies
        {
            get
            {
                IDictionary<string, IList<string>> list = InitializeCache();

                return list;
            }
        }
        /// <summary>
        /// See http://xlinux.nist.gov/dads//HTML/dictionary.html
        /// </summary>
        /// <returns></returns>
        private static IDictionary<string, IList<string>> InitializeCache()
        {
            IDictionary<string, IList<string>> dictionary = null;
            var myRepository = MyCache<Object>.CacheManager;
            if (myRepository != null)
            {
                dictionary = myRepository.GetData(cShareHolder) as IDictionary<string, IList<string>>;
                if (dictionary == null)
                {
                    //Initialize
                    dictionary = new Dictionary<string, IList<string>>();
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
            Companies.Add(shareHolder, new List<string>());
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
            //_listCompanies.Add(shareHolder, new List<string>());
            //_listCompanies[shareHolder].Add("123");
            //_listCompanies[shareHolder].Add("ah567");
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

    }
}
