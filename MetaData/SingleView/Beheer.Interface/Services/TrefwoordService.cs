using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Beheer.Interface.Services
{
    public class TrefwoordService : BusinessEntityServiceBase, ITrefwoordService
    {
        #region BusinessEntityServiceBase

        public override BeheerContextEntity GetMaster()
        {
            return Master as BeheerContextEntity;
        }
        //public override IList<BeheerContextEntity> GetEntities()
        //{
        //    var dataAccesLayer = new OracleAccess();

        //    m_BusinessEntities = dataAccesLayer.GetBusinessObjects("trefwoord", "trefwoord");
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

        //    dataAccesLayer.Insert(beheerContextEntity.Parent, 
        //        beheerContextEntity as BeheerContextEntity);
        //}
        //public override void UpdateBusinessEntity(IBeheerContextEntity beheerContextEntity)
        //{
        //    var dataAccesLayer = new OracleAccess();
        //    BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities,
        //        beheerContextEntity as BeheerContextEntity);
        //    dataAccesLayer.Update(found, beheerContextEntity as BeheerContextEntity);
        //}

        #endregion

    }
}