using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Repository
{
    public class ShareHolders
    {
        private const string cShareHolder ="shareholders";
        IDictionary<string, IList<string>> _listCompanies = new Dictionary<string, IList<string>>();
        IDictionary<string, string> _organiGraph = new Dictionary<string, string>();
        
        public ShareHolders()
        {
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
            var myRepository = MyCache<IDictionary<string, IList<string>>>.CacheManager;
            myRepository.Add(cShareHolder, _listCompanies);
        }

        public IList<string> GetSubsidiaries(string shareHolder)
        {
            if (string.IsNullOrEmpty(shareHolder))
                return null;
            var myRepository = MyCache<Object>.CacheManager;
            return (myRepository.GetData(cShareHolder) as IDictionary<string, IList<string>>)[shareHolder];

        }
        public IList<string> GetShareHolders(string subsidiary)
        {
            var myRepository = MyCache<Object>.CacheManager;
            _listCompanies = myRepository.GetData(cShareHolder) as IDictionary<string, IList<string>>;
            var listShareHolders  = from c in _listCompanies
                        where c.Value.Where(s => s.Equals(subsidiary)).FirstOrDefault() != null   
                        select c.Key;


            return listShareHolders.ToList<string>();
        }
        public void AddShareHolders(string shareHolder)
        {
            _listCompanies.Add(shareHolder, new List<string>());

            var myRepository = MyCache<Object>.CacheManager;
            if (myRepository!=null)
                MyCache<Object>.CacheManager.Add(cShareHolder, _listCompanies);
        }
        public void AddSubsidiary(string shareHolder, string subsidiary)
        {
            _listCompanies[shareHolder].Add(subsidiary);

            var myRepository = MyCache<Object>.CacheManager;
            if (myRepository != null)
                MyCache<IDictionary<string, IList<string>>>.CacheManager.Add(cShareHolder, _listCompanies); 
        }
        public void RemoveSubsidiary(string shareHolder, string subsidiary)
        {
            _listCompanies[shareHolder].Remove(subsidiary);

            var myRepository = MyCache<Object>.CacheManager;
            if (myRepository != null)
                MyCache<Object>.CacheManager.Add(cShareHolder, _listCompanies); 
        }

        public IList<string> GetRoot(string company)
        {
            IList<string> candidates = new List<string>();
            var myRepository = MyCache<Object>.CacheManager;
            _listCompanies = myRepository.GetData(cShareHolder) as IDictionary<string, IList<string>>;

            var parents = GetShareHolders(company);

            if (parents != null && parents.Count > 1)
            {
                candidates = GetShareHolders(company);

                foreach (var candidate in candidates)
                {
                    //find ancestores
                    //IList<string> ancestores = GetAncestors(candidate);
                }
                return candidates;
            }
            else//the company is the parent and root
                candidates = parents;

            
            return candidates;
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
