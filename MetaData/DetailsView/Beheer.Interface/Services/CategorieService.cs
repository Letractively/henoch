namespace MetaData.Beheer.Interface.Services
{
    public class CategorieService : BusinessEntityServiceBase, ICategorieService
    {
        //TODO: overide base voor realisisatie van de enitities
        //NOTE: gebruik een resourcefactory, als de controller nieuwe services nodig heeft voor presenters:
        //      var resourceFacory = new ResourceFactory<CategorieService>();
        //      var context = resourceFacory.Context;
        //      var resourceFacory = new ResourceFactory<CategorieService>();
        //      var context = resourceFacory.Context;
        //      var details = context.SetDetails(new BeheerContextEntity{DataKeyValue = "key"});
        //      foreach (var beheerContextEntity in details)
        //      {
        //          //Doe iets
        //      }

        #region  BusinessEntityServiceBase

        //public override IList<BeheerContextEntity> GetEntities()
        //{
        //    var dataAccesLayer = new OracleAccess();

        //    m_BusinessEntities = dataAccesLayer.GetBusinessObjects("categorie", "categorienaam");
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

        #endregion



    }
}