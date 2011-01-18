using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using ExceptionHandler;
using Maintenance;


namespace DataResource.Maintenance
{
    /// <summary>
    /// De tracefile is afhankelijk van de tracerbullet.dll.config in %HomePath%.
    /// De unittests gebruiken de trace- en dumpfile voor snelle feedback. Hierbij 
    /// worden de trace en de dumpfile ook getoond in console.
    /// </summary>
    public sealed class Log
    {
        Log()
        {
	        
        }

        private static string AppName;

        public static string DllName { get; private set; }

        public static TraceSwitch DebugLevel { get; private set; }

        public static TraceSwitch GetTraceLevel { get; private set; }


        /// <summary>
        /// Get the assembly location.
        /// </summary>
        public static string GetLogFile { get; private set; }

        public static string GetErrorFile { get; private set; }

        /// <summary>
        /// Initializes the trace/error loggers.
        /// </summary>
        public static void StartLogging(string dllName)
        {
            DllName = dllName;
            string defaultTraceLevel;
            string debugTraceLevel = String.Empty;
            try
            {
                defaultTraceLevel = Utility.GetappSetting("DefaultTracelevel");
                debugTraceLevel = Utility.GetappSetting("DebugTraceLevel");
            }
            catch (CheckedException)
            {
                defaultTraceLevel = "Info";
            }
            GetTraceLevel = new TraceSwitch("General", "The Output level of tracing");
            DebugLevel = new TraceSwitch("Debug", "The Output Debuglevel of tracing");
            // define the datastore for application specific files
			
            string applicationPath;
            //dllName = Assembly.GetExecutingAssembly().Location;
            applicationPath = Path.GetDirectoryName(dllName);
            if (applicationPath.ToUpperInvariant().Contains(@"C:\WINDOWS\MICROSOFT.NET"))
                applicationPath = Utility.HomePath;
            Debug.WriteLine(applicationPath);

            // Create Directory if it doesn't exists
            Directory.CreateDirectory(applicationPath);
            Directory.CreateDirectory(Path.Combine(applicationPath, "log"));
            // define the logging destination
            string shortName = Assembly.GetExecutingAssembly().Location;
            AppName = Path.GetFileNameWithoutExtension(shortName);
            string traceFile = Path.Combine(applicationPath, string.Format(CultureInfo.InvariantCulture,"log\\{0} Trace.log", AppName));
            GetErrorFile = Path.Combine(applicationPath, string.Format(CultureInfo.InvariantCulture,"log\\{0} Error.log", AppName));

            Debug.WriteLine(traceFile);
            Console.WriteLine("traceFile = " + traceFile);
            if (GetTraceLevel.Level == TraceLevel.Off || GetTraceLevel.Level == TraceLevel.Error
                || GetTraceLevel.Level == TraceLevel.Warning)				
            {
                Console.WriteLine("TraceLevel set to default.");
                Debug.WriteLine("TraceLevel set to default.");

                GetTraceLevel = new TraceSwitch("General", "The Output level of tracing", defaultTraceLevel);
                DebugLevel = new TraceSwitch("Debug", "The Output Debuglevel of tracing", debugTraceLevel);
                //if (m_generalLevel.TraceInfo) InitTraceLog(traceFile);//write application tracing messages.

                GetLogFile = null;
                GetErrorFile = null;

                InitTraceLog(traceFile);
            }
            else
            {
                // Use custom logfile and TraceListener if info or verbose was set.
                Console.WriteLine("TraceLevel set to info or lower.");
                InitTraceLog(traceFile);
            }
            //CheckTraceLevels();
        }
        /// <summary>
        /// Configures the trace-listeners.
        /// </summary>
        /// <param name="traceFile"></param>
        private static void InitTraceLog(string traceFile)
        {
            // Configure TraceListener
            string fullName = traceFile; //System.Reflection.Assembly.GetExecutingAssembly().FullName;
            Console.WriteLine("ExecutingAssembly = {0}", Assembly.GetExecutingAssembly().FullName);
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(traceFile, "NUnitTraceListener"));

            Trace.WriteIf(GetTraceLevel.TraceInfo, "\r\n");
            Trace.WriteLineIf(GetTraceLevel.TraceInfo,
                              "***********************************************************************************************************************************");
            Trace.WriteLineIf(GetTraceLevel.TraceInfo, string.Format(CultureInfo.InvariantCulture,"{0}: Application startup: {1} .", DateTime.Now, fullName));
            Trace.WriteLineIf(GetTraceLevel.TraceInfo,
                              "***********************************************************************************************************************************");
            Trace.WriteLine("TraceLevel.Off = " + (GetTraceLevel.Level == TraceLevel.Off));
            Trace.WriteLine("TraceError = " + (GetTraceLevel.Level == TraceLevel.Error));
            Trace.WriteLine("Warning = " + (GetTraceLevel.Level == TraceLevel.Warning));
            Trace.WriteLine("Info = " + (GetTraceLevel.Level == TraceLevel.Info));
            Trace.WriteLine("Verbose = " + (GetTraceLevel.Level == TraceLevel.Verbose));
        }

        public static bool IsTraceInfo()
        {
            return GetTraceLevel.TraceInfo;
        }

        public static bool IsTraceError()
        {
            return GetTraceLevel.TraceError;
        }

        /// <summary>
        /// This tracerror is used for logging errors. It traces error if traceswitch level is correct.
        /// </summary>
        /// <param name="ex"></param>
        public static void TraceError(Exception ex)
        {
            string message;
            message = string.Format(CultureInfo.InvariantCulture,"message:\t{0}\r\nSource:\t{1}\r\nStacktrace:\t{2}", ex.Message, ex.Source, ex.StackTrace);
            message = string.Format(CultureInfo.InvariantCulture,"{0}:{1} : {2}.",
                                    DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture), DateTime.Now.Millisecond,
                                    message);

            Trace.WriteLineIf(GetTraceLevel.TraceInfo, message); // the error message    
            Console.WriteLine(message);
        }

        /// <summary>
        /// This tracerror is used for logging errors. It traces messages if traceswitch level is correct.
        /// </summary>
        /// <param name="message"></param>
        public static void TraceError(string message)
        {
            Trace.WriteLineIf(GetTraceLevel.TraceInfo, string.Format(CultureInfo.InvariantCulture,"{0}:{1} : {2}.",
                                                                     DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture), 
                                                                     DateTime.Now.Millisecond,
                                                                     message)); // the error message            
        }
        /// <summary>
        /// This trace is used for logging messages. It writes messages if traceswitch level is correct.
        /// </summary>
        /// <param name="message"></param>
        public static void TraceMessage(string message)
        {
            if (GetTraceLevel != null)
            {
                Trace.WriteLineIf(GetTraceLevel.TraceInfo, string.Format(CultureInfo.InvariantCulture,"{0}:{1} : {2}.",
                                                                         DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture), 
                                                                         DateTime.Now.Millisecond,
                                                                         message)); // the error message            
            }
            else
            {
                //level not set => always trace.
                //Debug.WriteLine(message);
                Trace.WriteLine(message);
            }
        }
        public static bool IsDebugTraceError()
        {
            bool isDebugTraceError;
            try
            {
                isDebugTraceError = DebugLevel.TraceError;
            }
            catch (Exception ex)
            {
                string message =
                    string.Format(CultureInfo.InvariantCulture,"message:\t{0}\r\nSource:\t{1}\r\nStacktrace:\t{2}", ex.Message, ex.Source, ex.StackTrace);
                Trace.WriteLine(message);
                throw;
            }
            return isDebugTraceError;
        }
        /// <summary>
        /// Remove listener. Exceptions ignored.
        /// </summary>
        public static void StopLogging()
        {
            try
            {
                Trace.Listeners.Remove("NUnitTraceListener");
            }
            finally 
            {
                GetTraceLevel = null;
                DebugLevel = null;
            }

        }

        /// <summary>
        /// Traces message, Source and Stacktrace in trace file (no sessionlogs used).
        /// </summary>
        /// <param name="ex"></param>
        public static void TraceWriteLineException(Exception ex)
        {
            string message = string.Format(CultureInfo.InvariantCulture,"message:\t{0}\r\nSource:\t{1}\r\nStacktrace:\t{2}",
                                           ex.Message, ex.Source, ex.StackTrace);
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture,"{0}:{1} : {2}.", 
                                          DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture), DateTime.Now.Millisecond, message));
        }

        /// <summary>
        /// Writes to Console and Trace file.
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleWriteline(string message)
        {
            Trace.WriteLine(message);
            Console.Out.WriteLine(message);
        }
        public static void Writeline(object value)
        {
            Trace.WriteLine(value);
            ///Console.Out.WriteLine(value);
        }
        public static void Writeline(string format, params object[] args)
        {
            string txt = string.Format(format, args);
            Trace.WriteLine(txt);
            string message = string.Format(format, args);
            if (GetTraceLevel != null)
            {
                if (args != null)
                    Trace.WriteLineIf(GetTraceLevel.TraceInfo, string.Format(CultureInfo.InvariantCulture, "{0}:{1} : {2}.",
                                                                             DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
                                                                             DateTime.Now.Millisecond,
                                                                             message)); // the error message            
            }
            else
            {
                //level not set => always trace.
                //Debug.WriteLine(message);
                Trace.WriteLine(message);
            }
        }
        public static bool Dump<TKey, TValue>(IDictionary<TKey, TValue> listWaarden)
        {
            try
            {
                foreach (var w in listWaarden)
                {
                    Writeline(w.Value);
                }
            }
            catch (Exception ex)
            {
                TraceError(ex);
                throw;
            }
            return true;
        }
        public static int Dump<T>(List<T> list)
        {
            Console.Out.WriteLine("+++++++++++");
            int i = -1;
            if (list != null)
                foreach (var item in list)
                {
                    Console.Out.WriteLine(item);
                    Trace.WriteLine(item);
                    i++;
                }
            return i;
        }
        public static void ConsoleWrite(string message)
        {
            Trace.Write(message);
            Console.Write(message);
        }


        public static void Dump<T>(IList list)
        {
            Console.Out.WriteLine("+++++++++++");
            int i = -1;
            foreach (var item in list)
            {
                Console.Out.WriteLine(item);
                Trace.WriteLine(item);
                i++;
            }
        }
    }
}