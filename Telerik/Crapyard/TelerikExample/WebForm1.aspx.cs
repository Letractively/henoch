using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TelerikExample.LondonGoldSilverFix;
using TelerikExample.StockQuote;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text;

namespace TelerikExample
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private Telerik.Charting.ChartSeries seriegold;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            
            //Response.Redirect("~/WebForm1.aspx");
            //Server.Transfer("~/PersistExpandedState20.aspx");
            
            //LondonGoldAndSilverFixSoap client = new LondonGoldAndSilverFixSoapClient();
            //var response = client.GetLondonGoldAndSilverFix(new GetLondonGoldAndSilverFixRequest());
            //var data = response.GetLondonGoldAndSilverFixResult;

            StockQuoteSoapClient client = new StockQuoteSoapClient();
            var data = client.GetQuote("GOOG");
             // convert string to stream            
            byte[] byteArray = Encoding.ASCII.GetBytes(data);            
            MemoryStream stream = new MemoryStream( byteArray );
            XmlReader reader = XmlReader.Create(stream);
            XDocument doc = XDocument.Load(reader);

            var stockQuotes = from sq in doc.Root.Elements("Stock").Elements("Open")

                              select sq;
            double val;
            double.TryParse(stockQuotes.First().Value, out val);
            RadChart1.Series[0].AddItem(val);
        }
    }

}