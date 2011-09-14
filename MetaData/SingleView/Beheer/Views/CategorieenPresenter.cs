using System;
using System.Collections.Generic;
using System.Linq;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace MetaData.Beheer.Views
{

    public class CategorieenPresenter : Presenter<IBusinessEntityView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private ICategorieController _controller;
        public StateValue<bool> Showfooter;
        public StateValue<string> ErrorMessage;
        public StateValue<BeheerContextEntity> DuplicateMaster;
        public StateValue<BeheerContextEntity> DuplicateDetail;
        public StateValue<bool> IsSortedAscending;
        /// <summary>
        /// Geeft aan welke view gesorteerd is: master of details.
        /// </summary>
        public StateValue<bool> IsMasterSorted;
        public StateValue<IList<BeheerContextEntity>> m_ListDetails;
        public StateValue<bool> IsInsertingInline;

        public CategorieenPresenter([CreateNew] ICategorieController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.IsInsertingInline = IsInsertingInline.Value;
            IsInsertingInline.Value = false;

            //Dit is voor een lege gridview.
            var dummy = new BeheerContextEntity
                            {
                                DataKeyValue = "leeg",
                                Master = "leeg",
                                Id = -2 //-1 = geen trefwoord, -2 = lege tabel, -3 = nieuw trefwoord.
                            };
            var listCategorieen = _controller.GetEntities();

            m_ListDetails.Value = _controller.GetDetails();


            var found = BusinessEntityServiceBase.FindBusinessEntity(m_ListDetails.Value, dummy);
            if (found == null && listCategorieen.Count==0)
            {
                m_ListDetails.Value.Add(dummy);
            }
            else
            {
                m_ListDetails.Value.Remove(found);
            }

            #region Voeg een default Detail toe als de er geen details zijn
            var masters = listCategorieen;
            foreach (var master in masters)
            {
                //Maak een detail met alleen een spatie en voeg deze toe.
                //De default is gereserveerd voor een insert.
                if(master.Details.Count==0)
                {
                    var defaultDetail = CreateDefaultDetail(master);
                    m_ListDetails.Value.Add(defaultDetail);
                }
                
            }
            #endregion


            View.BusinessEntities = listCategorieen;

            if (IsMasterSorted.Value)
            {
                if(IsSortedAscending.Value)
                {
                    OnSortAscending();
                }
                else
                {
                    OnSortDescending();
                }
            }
            else
            {
                if (IsSortedAscending.Value)
                {
                    OnSortAscendingDetails();
                }
                else
                {
                    OnSortDetailsAscending();
                }
            }
        }
        /// <summary>
        /// Creeert een detail met spatie waarde.
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private BeheerContextEntity CreateDefaultDetail(BeheerContextEntity master)
        {
            BeheerContextEntity detail = new BeheerContextEntity();
            detail.DataKeyValue = " ";

            //-1 = geen trefwoord, -2 = lege tabel, -3 = nieuw trefwoord.
            detail.Id = -1;//-1 betekent: bestaat niet. 0 is de laagste waarde voor indices.

            detail.Parent = new ParentKeyEntity
            {
                DataKeyValue = master.DataKeyValue,
                Id = master.Id
            };

            detail.MasterId = master.Id;
            detail.Master = master.DataKeyValue;
            return detail;
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            IsInsertingInline.Value =true;
            IsSortedAscending.Value = true;
            IsMasterSorted.Value = true;
            m_ListDetails = new StateValue<IList<BeheerContextEntity>>
            {
                Value = new List<BeheerContextEntity>()
            };
        }

        // TODO: Handle other view events and set state in the view
        public void OnBusinessEntityUpdated(BeheerContextEntity entity)
        {
            _controller.UpdateBusinessEntity(entity);
        }

        public void OnBusinessEntityAdded(BeheerContextEntity entity)
        {
            _controller.AddBusinessEntity(entity);
        }

        public void OnBusinessEntityDeleted(BeheerContextEntity entity)
        {
            _controller.DeleteBusinessEntity(entity);
        }


        public void OnSelectedEntity(BeheerContextEntity entity)
        {
            _controller.Selected = entity;            
            _controller.UpdateDetails(entity);
            _controller.AllowCrud = true;
        }

        public void OnAddingMaster(BeheerContextEntity master)
        {
            if (master != null)
            {
                _controller.AddMaster(master);
            }
        }


        public void OnDetailEntityAdded(BeheerContextEntity detail)
        {
            _controller.UpdateDetailEntity(detail);
        }



        public void OnDetailEntityDeleted(BeheerContextEntity detail)
        {
            _controller.DeleteDetailEntity(detail);
        }

        public void OnDetailEntityUpdated(BeheerContextEntity detail)
        {
            _controller.UpdateDetailEntity(detail);
        }

        public void OnViewShowfooter(bool showFooter)
        {
            Showfooter= new StateValue<bool>(showFooter);
            View.ShowFooter = Showfooter.Value;
        }

        public void OnViewShowErrorMessage(string errorMessage)
        {
            ErrorMessage = new StateValue<string>(errorMessage);
           
        }

        /// <summary>
        /// Sort master
        /// </summary>
        public void OnSortAscending()
        {
            if (m_ListDetails != null && m_ListDetails.Value !=null)
                View.DetailsEntities = m_ListDetails.Value.
                    OrderBy(detail => detail.Master)
                    .ThenBy(detail => detail.Id).ToList();
            IsSortedAscending.Value = true;
            IsMasterSorted.Value = true;
        }
        /// <summary>
        /// Sort master
        /// </summary>
        public void OnSortDescending()
        {
            if (m_ListDetails != null && m_ListDetails.Value != null)
                View.DetailsEntities = m_ListDetails.Value.
                    OrderByDescending(detail => detail.Master)
                    .ThenBy(detail => detail.Id).ToList();
            IsSortedAscending.Value = false;
            IsMasterSorted.Value = true;
        }
        /// <summary>
        /// Sort details
        /// </summary>
        public void OnSortAscendingDetails()
        {
            if (m_ListDetails != null && m_ListDetails.Value != null)
                View.DetailsEntities = m_ListDetails.Value.
                    OrderBy(detail => detail.DataKeyValue)
                    .ToList();
            IsSortedAscending.Value = true;
            IsMasterSorted.Value = false;
        }
        /// <summary>
        /// Sort details
        /// </summary>
        public void OnSortDetailsAscending()
        {
            if (m_ListDetails != null && m_ListDetails.Value != null)
                View.DetailsEntities = m_ListDetails.Value.
                    OrderByDescending(detail => detail.DataKeyValue)
                    .ToList();
            IsSortedAscending.Value = false;
            IsMasterSorted.Value = false; 
        }

        public void OnInlineDetailInsert()
        {
            IsInsertingInline.Value = true;
        }
    }
}




