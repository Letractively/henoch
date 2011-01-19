using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ApplicationTypes.DesignPatterns;

namespace Observlet
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            /// Represents a multi-threaded (cross-process) approach of the application.
            /// 
            Application["MTA-Sessions"] = new Dictionary<string, MySession>();

            // Create a trace listener for Web forms.
            WebPageTraceListener gbTraceListener = new WebPageTraceListener();
            // Add the event log trace listener to the collection.
            System.Diagnostics.Trace.Listeners.Add(gbTraceListener);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            
            // Code that runs when a new session is started
            IDictionary<string, MySession> applicationSessions = new Dictionary<string, MySession>();
            HttpSessionState session = HttpContext.Current.Session; 
            applicationSessions = Application["MTA-Sessions"] as Dictionary<string, MySession>;

            if (session != null && applicationSessions!=null)
                applicationSessions.Add(
                    new KeyValuePair<string, MySession>(session.SessionID, 
                    new MySession(){SessionId = session.SessionID, TaskQIsEmtpty = true}));

            //update applicationSessions
            Application["MTA-Sessions"] = applicationSessions;
            Session["m_Locker"] = new object();
            Application.UnLock();            
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}