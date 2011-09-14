using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using OraAccessLayer;

namespace MetaData.Beheer.Interface.Services
{
    public class BeheerService: BusinessEntityServiceBase
    {
        //public override IList<BeheerContextEntity> GetEntities()
        //{
        //    var dataAccesLayer = new OracleAccess();

        //    m_BusinessEntities = dataAccesLayer.GetBusinessObjects("thema", "themanaam");
        //    return m_BusinessEntities;
        //}

        //public override void DeleteBusinessEntity(IBeheerContextEntity beheerContextEntity)
        //{
        //    var dataAccesLayer = new OracleAccess();

        //    dataAccesLayer.Delete(beheerContextEntity as BeheerContextEntity);
            
        //}

        //public override void AddBusinessEntity(IBeheerContextEntity beheerContextEntity)
        //{
        //    var dataAccesLayer = new OracleAccess();

        //    dataAccesLayer.Insert(beheerContextEntity as BeheerContextEntity);
        //}
        //public override void UpdateBusinessEntity(IBeheerContextEntity beheerContextEntity)
        //{
        //    var dataAccesLayer = new OracleAccess();
        //    BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, 
        //        beheerContextEntity as BeheerContextEntity);
        //    dataAccesLayer.Update(found, beheerContextEntity as BeheerContextEntity);
        //}
        
    }
}