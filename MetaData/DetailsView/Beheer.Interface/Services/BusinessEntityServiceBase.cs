using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationTypes.ErrorHandler;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Beheer.Interface.Services
{
    /// <summary>
    /// Dit zorgt voor de toegang tot de repository.
    /// </summary>
    public class BusinessEntityServiceBase : IBeheerService
    {
        protected int m_Id;
        protected int m_IdMaster;
        protected IList<BeheerContextEntity> m_BusinessEntities = new List<BeheerContextEntity>();
        protected IList<BeheerContextEntity> m_DetailsLastUpdated;
        protected IDictionary<int, BeheerContextEntity> m_QueueMasters = new Dictionary<int, BeheerContextEntity>();
        private int m_NextMasterIndex;

        public bool AllowCrud { get; set; }
        public BeheerContextEntity Selected { get; set; }
        public BeheerContextEntity SelectedMaster { get; set; }

        public long Id { get; set; }
        public string DataKeyName { get; set; }
        public string TableName { get;private  set; }
        public IDictionary<string, AttributeValue> Attributes { get; set; }

        public virtual IList<BeheerContextEntity> GetEntities()
        {
            return m_BusinessEntities;
        }

        public IList<BeheerContextEntity> GetDetailsLastUpdated()
        {
            return m_BusinessEntities;
        }

        public virtual BeheerContextEntity GetMaster()
        {
            if (m_QueueMasters != null && 
                m_BusinessEntities!=null &&
                m_QueueMasters.Count > 0 && 
                m_NextMasterIndex < m_QueueMasters.Count)
            {
                var master = m_QueueMasters[m_NextMasterIndex];
                m_NextMasterIndex++;
                if (m_NextMasterIndex > m_QueueMasters.Count - 1)
                    m_NextMasterIndex = 0;//begin weer bij de eerste master.
                return master;
            }
            return null;
        }

        public IList<BeheerContextEntity> GetMasters()
        {
            IList<BeheerContextEntity> masters = new List<BeheerContextEntity>();
            foreach (var master in m_QueueMasters)
            {                
                masters.Add(master.Value);
            }
            return masters;
        }


        /// <summary>
        /// Alleen als de application-controller een select-proces in behandeling neemt van zijn presenter
        /// deze functie gebruiken.
        /// </summary>
        /// <param name="master"></param>
        public void UpdateDetails(BeheerContextEntity master)
        {
            m_DetailsLastUpdated = master.Details;

            AddMaster(master);
            Master = master;
        }
        /// <summary>
        /// De repository van masters inrichten en herinrichten, als de master al bestaat.
        /// </summary>
        /// <param name="master"></param>
        public void AddMaster(BeheerContextEntity master)
        {
            BeheerContextEntity found;
            m_QueueMasters.TryGetValue(master.Id, out found);
            if (m_QueueMasters != null && found == null)
            {
                m_QueueMasters.Add(new KeyValuePair<int, BeheerContextEntity>(master.Id, master));
            }
        }
        /// <summary>
        /// Current master.
        /// </summary>
        public IBeheerContextEntity Master { get; set; }

        public virtual void AddBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            // <pex>
            if (beheerContextEntity == null)
                throw new ArgumentNullException("beheerContextEntity");
            // </pex>
           
            if (beheerContextEntity.DataKeyValue.Equals("1"))
                throw new BusinessLayerException("duplicate");

            beheerContextEntity.Id = m_Id;

            m_BusinessEntities.Add(beheerContextEntity as BeheerContextEntity);

            if (beheerContextEntity.Parent != null)
            {
                //Deze entity heeft een parent.
                //Voeg de detail toe aan de master.                
                if (m_QueueMasters != null && m_QueueMasters.Count > 0 && beheerContextEntity.Parent != null)
                    m_QueueMasters[beheerContextEntity.Parent.Id].Details.
                        Add(beheerContextEntity as BeheerContextEntity);
            }
            m_Id++;
        }

        public void AddDetailBusinessEntity(BeheerContextEntity detail)
        {
            var master = m_BusinessEntities.Where(mas => mas.Id == detail.Parent.Id).FirstOrDefault();
            var details = master.Details;

            var found = FindBusinessEntity(details, detail);
            if (found == null)
            {
                detail.Id = m_Id;
                details.Add(detail);
                m_Id++;
            }
        }

        public virtual void DeleteBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            IBeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, beheerContextEntity as BeheerContextEntity);
            if (found != null)
            {
                m_BusinessEntities.Remove(found as BeheerContextEntity);
                //SelectedMaster.Details.Remove(found as BeheerContextEntity);
                if (beheerContextEntity.Parent != null)
                {
                    if (m_QueueMasters != null && m_QueueMasters.Count > 0 && beheerContextEntity.Parent != null)
                        m_QueueMasters[beheerContextEntity.Parent.Id].Details.
                            Remove(beheerContextEntity as BeheerContextEntity);
                }
            }
        }

        public void DeleteDetailBusinessEntity(BeheerContextEntity detail)
        {
            var master = m_BusinessEntities.Where(mas => mas.DataKeyValue == detail.Master).FirstOrDefault();
            var details = master.Details;

            var found = FindBusinessEntity(details, detail);
            if (found != null)
            {
                details.Remove(found);
            }
        }

        public virtual void UpdateBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            // <pex>
            if (beheerContextEntity == null)
                throw new ArgumentNullException("beheerContextEntity");
            // </pex>
            BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, beheerContextEntity as BeheerContextEntity);
            if (found != null)
            {
                found.DataKeyValue = beheerContextEntity.DataKeyValue;
                found.Master = beheerContextEntity.Master;

                if (beheerContextEntity.Details.Count > 0)
                {
                    //Voeg details toe die nieuw zijn.
                    int count = beheerContextEntity.Details.Count;
                    for (int i = 0; i < count; i++)
                    {
                        BeheerContextEntity detail = beheerContextEntity.Details[i];
                        if (detail.Id == -3 ) 
                        {
                            detail.Id = m_Id++;
                            found.Details.Add(detail);
                        }
                    }
                    
                    //bijwerk de id's, waarvan de details updates zijn.
                    found.Details.
                       Where(detail => detail.Id == -1 || !detail.DataKeyValue.Equals(" ")).
                       Update(x =>
                       {
                           x.Id = m_Id++;
                           //x.DataKeyValue = beheerContextEntity.DataKeyValue;
                       });
                }
                BeheerContextEntity master;
                int idMaster = beheerContextEntity.Parent.Id;
                m_QueueMasters.TryGetValue(idMaster, out master);
                if (master!=null)
                {
                    //het is een master.Wijzig de master met nieuwe waarden.                    
                    m_QueueMasters[idMaster].DataKeyValue = beheerContextEntity.Parent.DataKeyValue;
                    
                    //update elke detail met veranderde master waarden.
                    m_BusinessEntities.
                        Where(detail => detail.Parent.Id == idMaster).
                        Update(x =>
                                   {
                                       x.Master = beheerContextEntity.Parent.DataKeyValue;
                                       //x.DataKeyValue = beheerContextEntity.DataKeyValue;
                                   });
                }

                if (beheerContextEntity.Parent != null)
                {
                    //het is een detail.
                    if (m_QueueMasters != null && m_QueueMasters.Count > 0 && beheerContextEntity.Parent!=null)
                        m_QueueMasters[beheerContextEntity.Parent.Id].Details.
                            Where(detail => detail.Id == found.Id).
                            Update(detail=> detail.DataKeyValue = found.DataKeyValue);
                }
            }
            
        }

        public void UpdateDetailBusinessEntity(BeheerContextEntity updatedDetail)
        {
            // <pex>
            if (updatedDetail == null)
                throw new ArgumentNullException("updatedDetail");
            // </pex>
            var master = m_BusinessEntities.Where(mas => mas.DataKeyValue == updatedDetail.Master).FirstOrDefault();
            var details = master.Details;

            var found = FindBusinessEntity(details, updatedDetail);
            if (found != null)
            {
                details.Where(detail => detail.Id == found.Id).
                                Update( detail =>
                                        {
                                            detail.DataKeyValue = updatedDetail.DataKeyValue;
                                            
                                        });
            }
        }


        public static BeheerContextEntity FindBusinessEntity(IList<BeheerContextEntity> entities,
                                                        BeheerContextEntity entity)
        {
            // <pex>
            if (Equals(entity, default(BeheerContextEntity)))
                throw new ArgumentNullException("entity");
            // </pex>
            IEnumerable<BeheerContextEntity> result;
            if (entities != null)
            {
                result = entities.Where(businessObject => businessObject.Id.Equals(entity.Id));
                return result.FirstOrDefault();             
            }
            return null;
        }
    }
}