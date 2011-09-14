using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using Rhino.Mocks;

namespace MetaData.Beheer.Tests.Mocks
{
    public class MockDataResource
    {
        
        /// <summary>
        /// Deze is om BO te vullen
        /// </summary>
        /// <returns></returns>
        public IList<Thema> GetThemaTableStub()
        {
            IList<Thema> list;
            MockRepository mocks = new MockRepository();
            list = new List<Thema>();

            ///Make 10 tables/Business Objects.
            for (int i = 0; i < 4; i++)
            {
                IThema table = mocks.Stub<IThema>();
                table.ThemaNaam = "thema-" + i;
                list.Add(new Thema { ThemaNaam = table.ThemaNaam });
            }
            return list;
        }
        public IList<BeheerContextEntity> GetBusinessEntitiesTableStub(string dataKeyName)
        {
            IList<BeheerContextEntity> list;
            MockRepository mocks = new MockRepository();
            list = new List<BeheerContextEntity>();

            ///Make 10 tables/Business Objects.
            for (int i = 0; i < 4; i++)
            {
                IBeheerContextEntity superKey = mocks.Stub<IBeheerContextEntity>();
                superKey.DataKeyValue = dataKeyName + "-" + i;
                BeheerContextEntity entity =new BeheerContextEntity
                                                {
                                                    Id = i,
                                                    DataKeyValue = superKey.DataKeyValue
                                                };
                entity.Attributes.Add("Id", new AttributeValue { ValueType = superKey.Id });
                entity.Attributes.Add("DataKeyValue", new AttributeValue { ValueStringType = superKey.DataKeyValue });
                entity.Attributes.Add("Kolom3", new AttributeValue { ValueType = DateTime.Now });
                entity.Attributes.Add("Kolom4", new AttributeValue { ValueType = 1.4d });
                entity.Attributes.Add("Kolom5", new AttributeValue { ValueStringType = "blah" });
                list.Add(entity);
            }
            return list;
        }
        public IList<TBusinessEntity> GetBusinessEntitiesTableStub<TBusinessEntity>(string dataKeyName) 
            where TBusinessEntity : IBeheerContextEntity, new()
        {
            IList<TBusinessEntity> list;
            MockRepository mocks = new MockRepository();
            list = new List<TBusinessEntity>();

            ///Maak Business Objects.
            for (int i = 0; i < 4; i++)
            {
                IBeheerContextEntity superKey = mocks.Stub<IBeheerContextEntity>();
                superKey.DataKeyValue = dataKeyName + "-" + i;
                TBusinessEntity entity = new TBusinessEntity
                                             {
                                                 Id = i,
                                                 DataKeyValue = superKey.DataKeyValue
                                             };
                //entity.Attributes.Add("Id", new AttributeValue { ValueType = superKey.Id });
                //entity.Attributes.Add("DataKeyValue", new AttributeValue { ValueStringType = superKey.DataKeyValue });
                //entity.Attributes.Add("Kolom3", new AttributeValue { ValueType = DateTime.Now });
                //entity.Attributes.Add("Kolom4", new AttributeValue { ValueType = 1.4d });
                //entity.Attributes.Add("Kolom5", new AttributeValue { ValueStringType = "blah" });
                list.Add(entity);
            }
            return list;
        }
        public IList<Categorie> GetCategorieTableStub()
        {
            IList<Categorie> list;
            MockRepository mocks = new MockRepository();
            list = new List<Categorie>();

            ///Make 10 tables/Business Objects.
            for (int i = 0; i < 4; i++)
            {
                ICategorie table = mocks.Stub<ICategorie>();
                table.Categorienaam = "Categorie-" + i;
                list.Add(new Categorie() { Categorienaam = table.Categorienaam });
            }
            return list;
        }

        public IList<Trefwoord> GetTrefwoordTableStub()
        {
            IList<Trefwoord> list;
            MockRepository mocks = new MockRepository();
            list = new List<Trefwoord>();

            ///Make 10 tables/Business Objects.
            for (int i = 0; i < 4; i++)
            {
                ITrefwoord table = mocks.Stub<ITrefwoord>();
                table.Trefwoordnaam = "Trefwoord-" + i;
                list.Add(new Trefwoord() { Trefwoordnaam = table.Trefwoordnaam });
            }
            return list;
        }
    }
}