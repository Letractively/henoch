using System;
using System.Web.Services;

namespace Observlet
{
    /// <summary>
    /// Summary description for MyService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MyService : WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetEmployee(string employeeId)
        {
            //simulate employee name lookup
            return "Jane Developer";
        }


        [WebMethod]
        public string ErrorFunction()
        {
            throw new ArgumentException("Muhahaha!");
            return "Hello World";
        }
    }
}
