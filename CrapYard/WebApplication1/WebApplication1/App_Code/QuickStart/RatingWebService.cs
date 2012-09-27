using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Configuration;

namespace Telerik.QuickStart
{
	[WebService(Namespace = "http://tempuri.org/")]//[WebService(Namespace="http://www.telerik.com/webservices/")]
	[ScriptService]
	public class RatingWebService : System.Web.Services.WebService
	{
		[WebMethod]
		public void SaveRating(object context)
		{
			IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
			double rating = Double.Parse((string)contextDictionary["rating"]);
			string demo = (string)contextDictionary["demo"];
			string ip = HttpContext.Current.Request.UserHostAddress;

			string checkRatedCommandText = "SELECT MAX([Rating]) from [Ratings] WHERE [IP]=@ip AND [Demo]=@demo";
			string updateCommandText = @"INSERT INTO [Ratings] ([IP], [Demo], [Rating], [Browser], [BrowserVersion], [Date]) " +
					"VALUES (@ip, @demo, @rating, @browser, @browserVersion, @date)";

			using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QSFRatingsConnectionString"].ConnectionString))
			{
				SqlCommand checkRated = new SqlCommand(checkRatedCommandText, connection);
				checkRated.Parameters.AddWithValue("@ip", ip);
				checkRated.Parameters.AddWithValue("@demo", demo);

				connection.Open();
				object qsfRating = checkRated.ExecuteScalar();
				if (DBNull.Value.Equals(qsfRating) || ((double)qsfRating == 0d))
				{
					SqlCommand insertCommand = new SqlCommand(updateCommandText, connection);
					insertCommand.Parameters.AddWithValue("@ip", ip);
					insertCommand.Parameters.AddWithValue("@demo", demo);
					insertCommand.Parameters.AddWithValue("@rating", rating);
					insertCommand.Parameters.AddWithValue("@browser", Context.Request.Browser.Browser);
					insertCommand.Parameters.AddWithValue("@browserVersion", Context.Request.Browser.Version);
					insertCommand.Parameters.AddWithValue("@date", DateTime.Now);

					insertCommand.ExecuteNonQuery();
				}
			}
		}

		[WebMethod]
		public void SaveComment(object context)
		{
			IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;
			string comment = (string)contextDictionary["comment"];
			string demo = (string)contextDictionary["demo"];

			string updateCommandText = @"INSERT INTO [Ratings] ([IP], [Demo], [Browser], [BrowserVersion], [Date], [Comment]) " +
					"VALUES (@ip, @demo, @browser, @browserVersion, @date, @comment)";

			using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["QSFRatingsConnectionString"].ConnectionString))
			{
				SqlCommand insertCommand = new SqlCommand(updateCommandText, connection);
				insertCommand.Parameters.AddWithValue("@ip", HttpContext.Current.Request.UserHostAddress);
				insertCommand.Parameters.AddWithValue("@demo", demo);
				insertCommand.Parameters.AddWithValue("@browser", Context.Request.Browser.Browser);
				insertCommand.Parameters.AddWithValue("@browserVersion", Context.Request.Browser.Version);
				insertCommand.Parameters.AddWithValue("@date", DateTime.Now);
				insertCommand.Parameters.AddWithValue("@comment", comment);

				connection.Open();
				insertCommand.ExecuteNonQuery();
			}
		}
	}
}

