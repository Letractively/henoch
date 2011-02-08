using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace AsyncHandlers
{
    public class ViewStatePersistor : Page
    {

        #region "Declarations"
        private static CacheManager _ViewState = CacheFactory.GetCacheManager();
        private Object _viewState;
        private string _CacheIdentifier;

        private const int DefaultViewStateTimeout = 20;
        private const string SP_GET_VIEWSTATE = "spGetViewState";
        private const string SP_SET_VIEWSTATE = "spSetViewState";
        private string _viewStateConnectionString;
        #endregion
        private TimeSpan _viewStateTimeout;

        #region "Initiator(s)"

        public ViewStatePersistor()
            : base()
        {
           
            if (this.IsDesignMode)
            {
                return;
            }
            //this._viewStateConnectionString = ConfigurationManager.ConnectionStrings["AMSConnectionString"].ConnectionString;
            //try
            //{
            //    this._viewStateTimeout = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["ViewStateTimeout"]));
            //}
            //catch
            //{
            //    this._viewStateTimeout = TimeSpan.FromMinutes(ViewStatePersistor.DefaultViewStateTimeout);
            //}
        }

        #endregion

        #region "Properties"
        /// <summary>
        /// Uses cachemanager to retrieve/store state.
        /// </summary>
        private Object ViewStateCached
        {
            get
            {
                _CacheIdentifier = "ViewStateCached" + Context.Session.SessionID;
                if (_ViewState.Contains(_CacheIdentifier))
                {
                    _viewState = _ViewState.GetData(_CacheIdentifier) ;
                }
                else
                {                    
                    _ViewState.Add(_CacheIdentifier, _viewState, CacheItemPriority.High, null, null);
                }
                return _viewState;
            }
            set
            {
                _CacheIdentifier = "ViewStateCached" + Context.Session.SessionID;
                _ViewState.Add(_CacheIdentifier, value, CacheItemPriority.High, null, null);
                _viewState = value;
            }
        }

        private bool IsDesignMode
        {
            get { return (this.Context == null); }
        }

        private bool IsInProcCacheStateEnabled
        {
            get { return (this._viewStateConnectionString != null && this._viewStateConnectionString.Length > 0); }
        }

        public TimeSpan ViewStateTimeout
        {
            get { return this._viewStateTimeout; }

            set { this._viewStateTimeout = value; }
        }


        #endregion

        #region "Methods"

        private LosFormatter GetLosFormatter()
        {
            return new LosFormatter();
        }

        private Guid GetViewStateGuid()
        {
            string viewStateKey = null;

            viewStateKey = this.Request.Form["__VIEWSTATEGUID"];

            if (viewStateKey == null || viewStateKey.Length < 1)
            {
                viewStateKey = this.Request.QueryString["__VIEWSTATEGUID"];
                if (viewStateKey == null || viewStateKey.Length < 1)
                {
                    return Guid.NewGuid();
                }
            }

            try
            {
                return new Guid(viewStateKey);
            }
            catch (FormatException generatedExceptionName)
            {
                return Guid.NewGuid();
            }
        }

        #region Overrides
        protected override object LoadPageStateFromPersistenceMedium()
        {
           
            if (this.IsDesignMode)
            {
                return null;
            }

            return ViewStateCached;
        }

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            Guid viewStateGuid = default(Guid);
            HtmlInputHidden control = default(HtmlInputHidden);

            if (this.IsDesignMode)
            {
                return;
            }

            viewStateGuid = this.GetViewStateGuid();

            ViewStateCached = viewState;

            control = this.FindControl("__VIEWSTATEGUID") as HtmlInputHidden;

            if (control == null)
            {
                ClientScript.RegisterHiddenField("__VIEWSTATEGUID", viewStateGuid.ToString());
            }
            else
            {
                control.Value = viewStateGuid.ToString();
            }
        }
        #endregion

        #endregion

    }
}
