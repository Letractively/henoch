using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationTypes.ErrorHandler;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Audittrail.Interface.Services;

namespace MetaData.Beheer.Interface.Services
{
    /// <summary>
    /// Dit zorgt voor de toegang tot de repository.
    /// </summary>
    public class RepositoryServiceBase : IPureFabricationRepoService
    {
        protected int m_Id;
        protected int m_IdMaster;
        protected IList<AuditItem> m_BusinessEntities = new List<AuditItem>();
        protected IList<AuditItem> m_DetailsLastUpdated;
        protected IDictionary<int, AuditItem> m_QueueMasters = new Dictionary<int, AuditItem>();
        private int m_NextMasterIndex;

        public bool AllowCrud { get; set; }
        public AuditItem Selected { get; set; }
        public AuditItem SelectedMaster { get; set; }

        public long Id { get; set; }
        public string DataKeyName { get; set; }
        public string TableName { get;private  set; }
        public IDictionary<string, AttributeValue> Attributes { get; set; }

        public virtual IList<AuditItem> GetEntities()
        {
            return m_BusinessEntities;
        }

        public IList<AuditItem> GetDetailsLastUpdated()
        {
            return m_BusinessEntities;
        }

        public virtual AuditItem GetMaster()
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

        public IList<AuditItem> GetMasters()
        {
            IList<AuditItem> masters = new List<AuditItem>();
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
        public void UpdateDetails(AuditItem master)
        {
            m_DetailsLastUpdated = master.Details;

            AddMaster(master);
            Master = master;
        }
        /// <summary>
        /// De repository van masters inrichten en herinrichten, als de master al bestaat.
        /// </summary>
        /// <param name="master"></param>
        public void AddMaster(AuditItem master)
        {
            AuditItem found;
            m_QueueMasters.TryGetValue(master.Id, out found);
            if (m_QueueMasters != null && found == null)
            {
                m_QueueMasters.Add(new KeyValuePair<int, AuditItem>(master.Id, master));
            }
        }
        /// <summary>
        /// Current master.
        /// </summary>
        public IAuditItem Master { get; set; }

        public virtual void AddBusinessEntity(IAuditItem beheerContextEntity)
        {
            // <pex>
            if (beheerContextEntity == null)
                throw new ArgumentNullException("beheerContextEntity");
            // </pex>
           
            if (beheerContextEntity.DataKeyValue.Equals("1"))
                throw new BusinessLayerException("duplicate");

            beheerContextEntity.Id = m_Id;

            m_BusinessEntities.Add(beheerContextEntity as AuditItem);

            if (beheerContextEntity.Parent != null)
            {
                //Deze entity heeft een parent.
                //Voeg de detail toe aan de master.                
                if (m_QueueMasters != null && m_QueueMasters.Count > 0 && beheerContextEntity.Parent != null)
                    m_QueueMasters[beheerContextEntity.Parent.Id].Details.
                        Add(beheerContextEntity as AuditItem);
            }
            m_Id++;
        }

        public void AddDetailBusinessEntity(AuditItem detail)
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

        public virtual void DeleteBusinessEntity(IAuditItem beheerContextEntity)
        {
            IAuditItem found = FindBusinessEntity(m_BusinessEntities, beheerContextEntity as AuditItem);
            if (found != null)
            {
                m_BusinessEntities.Remove(found as AuditItem);
                //SelectedMaster.Details.Remove(found as AuditItem);
                if (beheerContextEntity.Parent != null)
                {
                    if (m_QueueMasters != null && m_QueueMasters.Count > 0 && beheerContextEntity.Parent != null)
                        m_QueueMasters[beheerContextEntity.Parent.Id].Details.
                            Remove(beheerContextEntity as AuditItem);
                }
            }
        }

        public void DeleteDetailBusinessEntity(AuditItem detail)
        {
            var master = m_BusinessEntities.Where(mas => mas.DataKeyValue == detail.Master).FirstOrDefault();
            var details = master.Details;

            var found = FindBusinessEntity(details, detail);
            if (found != null)
            {
                details.Remove(found);
            }
        }

        public virtual void UpdateBusinessEntity(IAuditItem beheerContextEntity)
        {
            // <pex>
            if (beheerContextEntity == null)
                throw new ArgumentNullException("beheerContextEntity");
            // </pex>
            AuditItem found = FindBusinessEntity(m_BusinessEntities, beheerContextEntity as AuditItem);
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
                        AuditItem detail = beheerContextEntity.Details[i];
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
                AuditItem master;
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

        public void UpdateDetailBusinessEntity(AuditItem updatedDetail)
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


        public static AuditItem FindBusinessEntity(IList<AuditItem> entities,
                                                        AuditItem entity)
        {
            // <pex>
            if (Equals(entity, default(AuditItem)))
                throw new ArgumentNullException("entity");
            // </pex>
            IEnumerable<AuditItem> result;
            if (entities != null)
            {
                result = entities.Where(businessObject => businessObject.Id.Equals(entity.Id));
                return result.FirstOrDefault();             
            }
            return null;
        }

    }

    public class AudittrailService : RepositoryServiceBase
    {
    }
}