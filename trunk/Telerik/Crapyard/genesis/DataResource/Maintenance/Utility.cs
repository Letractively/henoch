using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using DataResource;
using DataResource.Maintenance;
using ExceptionHandler;


namespace Maintenance
{
    /// <summary>
    /// Represents all statics and general utility of methods.
    /// </summary>
    public static class Utility
    {
        private static int m_GcCollectRequest;

        public static String SeparatorPeriod
        {
            get { return "."; }
        }

        public static String SeparatorEqual
        {
            get { return "="; }
        }

        public static String SeparatorQuotation
        {
            get { return "\""; }
        }

        public static String SeparatorQuotationSingle
        {
            get { return "\'"; }
        }

        public static String SeparatorAccoladeLeft
        {
            get { return "{"; }
        }

        public static String SeparatorAccoladeRight
        {
            get { return "}"; }
        }

        public static String SeperatorSpace
        {
            get { return " "; }
        }
        public static String SeparatorSharp
        {
            get { return "#"; }
        }
        /// <summary>
        /// \
        /// </summary>
        public static String SeparatorBackslash
        {
            get { return "\\"; }
        }
        /// <summary>
        /// /
        /// </summary>
        public static String SeparatorSlash
        {
            get { return "/"; }
        }
        public static String SeparatorColon
        {
            get { return ";"; }
        }

        public static String SeparatorTab
        {
            get { return "\t"; }
        }
        public static string DailyBuild
        {
            get { return "daily_build"; }
        }

        public static string Revision
        {
            get { return "revision"; }
        }
        public static string SettingsDesigner
        {
            get { return "Settings.Designer.cs"; }
        }
        /// <summary>
        /// An item with the same key has already been added.
        /// </summary>
        public static string ExceptionSameKey
        {
            get { return "An item with the same key has already been added."; }
        }
        /// <summary>
        /// Returns true if the line is a comment.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsVBCommentline(string line)
        {
            int ix = line.IndexOf(SeparatorQuotationSingle, StringComparison.Ordinal);
            if (ix < 0) ix = line.Length;
            string prefix = line.Substring(0, ix);
            return String.IsNullOrEmpty(prefix);
        }

        /// <summary>
        /// Gets the value from appSettings in .config .
        /// </summary>
        /// <exception cref="CheckedException"></exception>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetappSetting(string key)
        {
            string dllPath = Assembly.GetExecutingAssembly().Location;//this assembly tracerbullets.dll.
            if (dllPath != null)
                if (dllPath.ToUpperInvariant().Contains(@"C:\WINDOWS\MICROSOFT.NET"))
                    dllPath = HomePath+
                              Path.GetFileNameWithoutExtension(dllPath) + 
                              Path.GetExtension(dllPath);//only while debugging F5.
            string keyValue = "";
            try
            {
                #region obsolete
                ///Configuration config = ConfigurationManager.OpenExeConfiguration(@dllPath);
                ///Log.ConsoleWriteline(config.ToString());
                ///AppSettingsSection appSettings =
                ///    (AppSettingsSection) config.GetSection("appSettings");
                #endregion

                AppSettingsReader reader =
                                new AppSettingsReader();

                //NameValueCollection appStgs =
                //    ConfigurationManager.AppSettings;

                string[] names =
                    ConfigurationManager.AppSettings.AllKeys;

                if (names != null)
                {
                    ///appSettings.Settings[key].Value;
                    keyValue = (String)reader.GetValue(key, keyValue.GetType()); 
                }
                else
                {
                    throw new CheckedException(null);
                }
            }
            catch (CheckedException)
            {
                Log.ConsoleWriteline(string.Format(CultureInfo.InvariantCulture, "Het bestand (.config naast {0}) mist een element appSettings.",
                                                         AssemblyLocation));
            }
            catch (NullReferenceException)
            {
                Log.ConsoleWriteline(string.Format(CultureInfo.InvariantCulture, "Het bestand (.config naast {0}) mist een element key in appSettings: {1}",
                                                         AssemblyLocation, key));
            }
            catch (Exception ex)
            {
                Log.ConsoleWriteline(string.Format(CultureInfo.InvariantCulture, "Het lezen van bestand (.config naast {0}) veroorzaakt een error:\n{1}",
                                                         AssemblyLocation, ex.Message));
            }
            return keyValue;
        }

        public static void LogAppSettings()
        {


            AppSettingsReader reader =
                new AppSettingsReader();


            NameValueCollection appStgs =
                ConfigurationManager.AppSettings;

            string[] names =
                ConfigurationManager.AppSettings.AllKeys;

            String value = String.Empty;

            for (int i = 0; i < appStgs.Count; i++)
            {


                string key = names[i];

                value = (String) reader.GetValue(key, value.GetType());

                Log.Writeline("#{0} Name: {1} Value: {2}",
                              i, key, value);

            }
        }

        /// <summary>
        /// Assembly.GetExecutingAssembly().Location;
        /// </summary>
        public static string AssemblyLocation
        {
            get { return Assembly.GetExecutingAssembly().Location; }

        }

        /// <summary>
        /// Get key in App.config: DailyBuildRootName. At least String.Empty will be returned.
        /// </summary>
        /// <returns></returns>
        public static string GetAppKeyDailyBuildRootName()
        {
            string retVal;
            try
            {
                retVal = GetappSetting("DailyBuildRootName");
            }
            catch (Exception ex)
            {
                throw new CheckedException(ErrorType.EnvironmentFailure,
                                           string.Format(CultureInfo.InvariantCulture,"App.settings wordt niet goed gebruikt: {0}.", ex.Message));
            }
            return retVal;
        }

        /// <summary>
        /// Get key in App.config: DailyBuildRootNameTest. At least String.Empty will be returned.
        /// </summary>
        /// <returns></returns>
        public static string GetAppKeyDailyBuildRootNameTest()
        {
            string retVal;
            try
            {
                retVal = GetappSetting("DailyBuildRootNameTest");
            }
            catch (Exception ex)
            {
                throw new CheckedException(ErrorType.EnvironmentFailure,
                                           string.Format(CultureInfo.InvariantCulture,"App.settings wordt niet goed gebruikt: {0}.", ex.Message));
            }
            return retVal;
        }

        public static string GetHomeDrive()
        {
            string homeDrive;
            try
            {
                homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE") + "\\";
            }
            catch (Exception ex)
            {
                throw new CheckedException(ErrorType.IO,
                                           string.Format(CultureInfo.InvariantCulture,"Homedrive kan niet gevonden worden:\n{0}", ex.Message));
            }
            return homeDrive;
        }
        public static string HomePath
        {
            get
            {
                string homeDrive;
                string homePath;

                homeDrive = Environment.GetEnvironmentVariable("HOMEDRIVE") + "\\";
                homePath = homeDrive + Environment.GetEnvironmentVariable("HOMEPATH") + "\\";

                return homePath;
            }
        }
        /// <summary>
        /// Wrapper for Marshal.ReleaseComObject().
        /// </summary>
        /// <param name="comObject"></param>
        /// <param name="noFailures"></param>
        public static void ReleaseComObject(object value, ref bool noFailures)
        {
            string timeStamp = string.Format(CultureInfo.InvariantCulture,"{0}:{1}",
                                             DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
                                             DateTime.Now.Millisecond);
            int refsLeft;
            try
            {
                Debug.WriteLineIf(Log.GetTraceLevel.TraceInfo,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: Calling ReleaseComObject...", timeStamp));
                //references left;
                if (value != null)
                    do
                    {
                        refsLeft = Marshal.ReleaseComObject(value);
                        Debug.WriteLineIf(Log.GetTraceLevel.TraceInfo,
                                          string.Format(CultureInfo.InvariantCulture,"{0}:{1} : refsLeft = {2}.",
                                                        DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
                                                        DateTime.Now.Millisecond,
                                                        refsLeft));
                    } while (refsLeft > 0);
            }
            catch (Exception ex)
            {
                if (Log.GetTraceLevel.TraceInfo)
                {
                    Log.TraceWriteLineException(ex);
                    noFailures = false;
                }
                throw;
            }
            finally
            {
                Debug.WriteLineIf(Log.GetTraceLevel.TraceInfo,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: finally ReleaseComObject...", timeStamp));
            }
        }

        /// <summary>
        /// Wrapper for Marshal.FinalReleaseComObject().
        /// </summary>
        /// <param name="comObject"></param>
        /// <param name="noFailures"></param>
        public static void FinalReleaseComObject(object value, ref bool noFailures)
        {
            string timeStamp = string.Format(CultureInfo.InvariantCulture,"{0}:{1}",
                                             DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
                                             DateTime.Now.Millisecond);
            int refsLeft;
            try
            {
                Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: Calling FinalReleaseComObject...", timeStamp));
                //references left;
                if (value != null)
                    do
                    {
                        refsLeft = Marshal.FinalReleaseComObject(value);
                        Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                          string.Format(CultureInfo.InvariantCulture,"{0}:{1} : refsLeft = {2}.",
                                                        DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
                                                        DateTime.Now.Millisecond,
                                                        refsLeft));
                    } while (refsLeft > 0);
            }
            catch (Exception ex)
            {
                if (Log.GetTraceLevel.TraceInfo)
                {
                    Log.TraceWriteLineException(ex);
                    noFailures = false;
                }
                throw;
            }
            finally
            {
#pragma warning disable RedundantAssignment
                value = null;
#pragma warning restore RedundantAssignment
                Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: finally FinalReleaseComObject...", timeStamp));
            }
        }
        /// <summary>
        /// Wraps GC.Collect() and GC.WaitForPendingFinalizers(). Collects only when counter is modulo 10.
        /// </summary>
        public static void WaitForPendingFinalizers()
        {
            string timeStamp = string.Format(CultureInfo.InvariantCulture,"{0}:{1}",
                                             DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
                                             DateTime.Now.Millisecond);
            try
            {
                Debug.WriteLineIf(Log.DebugLevel.TraceInfo,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: GC.GetTotalMemory (force)= {1}",
                                                timeStamp, GC.GetTotalMemory(true)));
                Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: Calling WaitForPendingFinalizers...", timeStamp));
                Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: GC.Count = {1}, GC.MaxGen = {2} ",
                                                timeStamp, GC.CollectionCount(100),
                                                GC.MaxGeneration));

                if (m_GcCollectRequest % 2 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    Trace.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                      string.Format(CultureInfo.InvariantCulture,"{0}: requested => GC.Collect & GC.WaitForPendingFinalizers.",
                                                    timeStamp));
                }
            }
            catch (Exception ex)
            {
                if (Log.GetTraceLevel.TraceInfo)
                {
                    Log.TraceWriteLineException(ex);
                }
                throw;
            }
            finally
            {
                m_GcCollectRequest++;
                Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: GC.GetTotalMemory (force)= {1}",
                                                timeStamp, GC.GetTotalMemory(true)));
                Debug.WriteLineIf(Log.DebugLevel.TraceVerbose,
                                  string.Format(CultureInfo.InvariantCulture,"{0}: End WaitForPendingFinalizers. m_GcCollectRequest = {1}.",
                                                timeStamp,
                                                m_GcCollectRequest));
            }

        }
        /// <summary>
        /// Gets the prefix of word seperated by seperator.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string Prefix(string word, string seperator)
        {
            int ix = word.IndexOf(seperator, StringComparison.Ordinal);
            if (ix < 0) ix = 0;
            string prefix = word.Substring(0, ix); //word.IndexOf(seperator));
            return prefix;
        }
        /// <summary>
        /// Gets the Postfix of word seperated by seperator.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static string Postfix(string word, string seperator)
        {
            string postfix = word.Substring(word.IndexOf(seperator, StringComparison.Ordinal) + seperator.Length);
            return postfix;
        }

        /// <summary>
        /// Gets the Infix of word seperated by seperator.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Infix(string word, string begin, string end)
        {
            return Prefix(Postfix(word, begin), end);
        }

        /// <summary>
        /// Gets the Prefix of word seperated by seperator.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="seperator"></param>
        /// <param name="leftToRight"></param>
        /// <returns></returns>
        public static string Prefix(string word, string seperator, bool leftToRight)
        {
            string prefix;
            if (leftToRight)
                prefix = word.Substring(0, word.IndexOf(seperator, StringComparison.Ordinal));
            else
                prefix = word.Substring(0, word.LastIndexOf(seperator, StringComparison.Ordinal));
            return prefix;
        }
        /// <summary>
        /// Gets the Postfix of word seperated by seperator.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="seperator"></param>
        /// <param name="leftToRight"></param>
        /// <returns></returns>
        public static string Postfix(string word, string seperator, bool leftToRight)
        {
            string postfix;
            if (leftToRight)
                postfix = word.Substring(word.IndexOf(seperator, StringComparison.Ordinal) + seperator.Length);
            else
                postfix = word.Substring(word.LastIndexOf(seperator, StringComparison.Ordinal) + seperator.Length);
            return postfix;
        }
        /// <summary>
        /// Gets the Infix of word seperated by seperator.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="leftToRight"></param>
        /// <returns></returns>
        public static string Infix(string word, string begin, string end, bool leftToRight)
        {
            return Prefix(Postfix(word, begin, leftToRight), end, leftToRight);
        }
        /// <summary>
        /// Gets the filecount in path.
        /// </summary>
        /// <param name="path">The executing assembly path.</param>
        /// <param name="targetDir">The dir where svn-files will be downloaded.</param>
        /// <returns></returns>
        public static int GetFileCount(string path, string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(@path);
            DirectoryInfo[] dirs = dir.GetDirectories(targetDir);
            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture,"dir({1}). Total number of dirs : {0}.", dirs.Length, dir.FullName));
            int fileCount = 0;

            foreach (DirectoryInfo map in dirs)
            {
                try
                {
                    Trace.WriteLine(string.Format(CultureInfo.InvariantCulture,"Dir({0}) has {1} dirs.",
                                                  map.Name, map.GetDirectories().Length));
                    if (map.GetDirectories().Length > 0 | map.GetFiles().Length > 0)
                        fileCount = CountFiles(map);
                }
                catch (Exception ex)
                {
                    Log.TraceError(ex);
                    throw new CheckedException(ErrorType.IO, ex.Message);
                }
            }
            return fileCount;
        }
        /// <summary>
        /// Counts the files in map.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public static int CountFiles(DirectoryInfo map)
        {
            FileInfo[] files = map.GetFiles("*.*");

            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture,"Total number of files : {0}.", files.Length));
            return files.Length;
        }
        /// <summary>
        /// Returns the path of the assembly.
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyRootPath()
        {
            string rootDir;
            try
            {
                string dllName = Assembly.GetExecutingAssembly().Location;
                // unittest should be in the root-dir
                rootDir = Path.GetDirectoryName(dllName);
            }
            catch (Exception ex)
            {
                Log.TraceError(ex);
                Console.WriteLine("error = " + ex.Message);
                throw;
            }
            return rootDir;
        }



        /// <summary>
        /// Returns the dir where the files will be exported from a repository.
        /// </summary>
        /// <returns></returns>
        public static string ExportDir
        {
            get {return "svnexportedfiles"; }
        }
    }
}