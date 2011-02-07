using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace AsyncHandlers
{
    public abstract class ViewStatePersistor : Page
    {

        #region "Declarations"

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
            this._viewStateConnectionString = ConfigurationManager.ConnectionStrings["AMSConnectionString"].ConnectionString;
            try
            {
                this._viewStateTimeout = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["ViewStateTimeout"]));
            }
            catch
            {
                this._viewStateTimeout = TimeSpan.FromMinutes(ViewStatePersistor.DefaultViewStateTimeout);
            }
        }

        #endregion

        #region "Properties"

        protected bool IsDesignMode
        {
            get { return (this.Context == null); }
        }

        protected bool IsSqlViewStateEnabled
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
            Guid viewStateGuid = default(Guid);
            byte[] rawData = null;

            if (this.IsDesignMode)
            {
                return null;
            }

            if (!this.IsSqlViewStateEnabled)
            {
                return base.LoadPageStateFromPersistenceMedium();
            }

            viewStateGuid = this.GetViewStateGuid();
            rawData = null;

            using (SqlConnection connection = new SqlConnection(this._viewStateConnectionString))
            {
                using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings[SP_GET_VIEWSTATE], connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add("@viewStateId", SqlDbType.UniqueIdentifier).Value = viewStateGuid;

                    connection.Open();

                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                rawData = (byte[])Array.CreateInstance(typeof(byte), reader.GetInt32(0));
                            }
                            if (reader.NextResult() && reader.Read())
                            {
                                reader.GetBytes(0, 0, rawData, 0, rawData.Length);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //LogException("BaseSqlViewStatePage.LoadPageStateFromPersistenceMedium ERROR", ex, Log.LogSeverity.lgsDataBaseError);
                    }
                }
            }

            using (MemoryStream stream = new MemoryStream(rawData))
            {
                return this.GetLosFormatter().Deserialize(stream);
            }
        }

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            Guid viewStateGuid = default(Guid);
            HtmlInputHidden control = default(HtmlInputHidden);

            if (this.IsDesignMode)
            {
                return;
            }

            if (!this.IsSqlViewStateEnabled)
            {
                base.SavePageStateToPersistenceMedium(viewState);
                return;
            }

            viewStateGuid = this.GetViewStateGuid();
            using (MemoryStream stream = new MemoryStream())
            {
                this.GetLosFormatter().Serialize(stream, viewState);
                using (SqlConnection connection = new SqlConnection(this._viewStateConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(ConfigurationManager.AppSettings[SP_SET_VIEWSTATE], connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add("@viewStateId", SqlDbType.UniqueIdentifier).Value = viewStateGuid;
                        command.Parameters.Add("@value", SqlDbType.Image).Value = stream.ToArray();
                        command.Parameters.Add("@timeout", SqlDbType.Int).Value = this._viewStateTimeout.TotalMinutes;

                        connection.Open();

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            //LogException("BaseSqlViewStatePage.SavePageStateToPersistenceMedium ERROR", ex, Log.LogSeverity.lgsDataBaseError);
                        }

                    }
                }
            }
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
