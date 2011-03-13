
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using MxSystemsLib.System;


namespace DataResource.Patterns
{
    /// <summary>
    /// Represents the access to the dataresource (db, file, collection).
    /// </summary>
    public class MyAccess:IDataConsumer
    {
        private static IDictionary<string, double[]> m_OverschrijdingsKansenPerLocatie = new Dictionary<string, double[]>();

        public static IDictionary<string, double[]> OverschrijdingsKansenPerLocatie
        {
            get { return MyAccess.m_OverschrijdingsKansenPerLocatie; }
            set { MyAccess.m_OverschrijdingsKansenPerLocatie = value; }
        }

        public bool SaveBronBestand(int gegevensetId, string naam)
        {
            throw new NotImplementedException();
        }

        public Collection<string> ReadFile(string filename)
        {
            throw new NotImplementedException();
        }

        int IDataConsumer.SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, string format, int countSpacesPostFix)
        {
            return SaveToDatFile(datFile, matrix, rows, colums, format, countSpacesPostFix);
        }

        public bool Open(string fileNaam)
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public string FileName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Saves the matrix to a datFile. countSpacesPostFix indicates the number of spaces 
        /// between the values per line.
        /// </summary>
        /// <param name="datFile"></param>
        /// <param name="matrix"></param>
        /// <param name="rows"></param>
        /// <param name="colums"></param>
        /// <param name="format"></param>
        /// <param name="countSpacesPostFix"></param>
        /// <returns></returns>
        public static int SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, string format,
                                        int countSpacesPostFix)
        {
            int lineCount = 0;
            try
            {
                using (StreamWriter sw = new StreamWriter(datFile))
                {

                    for (int i = 0; i < rows; i++)
                    {
                        string line = "";
                        lineCount++;
                        line = (i + 1).ToString().PrefixThisWithSpaces(1);
                        line = line.PostfixThisWithSpaces(countSpacesPostFix);
                        for (int j = 0; j < colums; j++)
                        {
                            double value = matrix[i][j];
                            string svalue = string.Format(CultureInfo.InvariantCulture, format, value);
                            line += string.Format(CultureInfo.InvariantCulture, "{0}", svalue);
                            line = line.PostfixThisWithSpaces(countSpacesPostFix);
                        }
                        ///File.AppendAllText(datFile, line.Trim());
                        sw.WriteLine(line.Trim());
                    }
                }
            }
            catch (CheckedException ex)
            {
                throw new CheckedException(ErrorType.ProcessFailure, "Saving file failed:\n\r:" + ex.Message);
            }
            return lineCount;
        }
        /// <summary>
        /// format = collection of string formats (Precisions),  countPostFixes is the amount of seperators as postfixes.
        /// </summary>
        /// <param name="datFile"></param>
        /// <param name="matrix"></param>
        /// <param name="rows"></param>
        /// <param name="colums"></param>
        /// <param name="format"></param>
        /// <param name="countPostFixes"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static int SaveToDatFile(string datFile, double[][] matrix, int rows,
                                        int colums, Collection<string> format, int countPostFixes, string seperator)
        {
            int lineCount = 0;
            try
            {
                using (StreamWriter sw = new StreamWriter(datFile))
                {

                    for (int i = 0; i < rows; i++)
                    {
                        string line = "";
                        lineCount++;
                        line = (i + 1).ToString().PrefixThisWithSpaces(1);
                        line = line.PostfixThisWithSeperators(countPostFixes, seperator);
                        for (int j = 0; j < colums; j++)
                        {
                            double value = matrix[i][j];
                            string svalue = string.Format(CultureInfo.InvariantCulture, format[j], value);
                            line += string.Format(CultureInfo.InvariantCulture, "{0}", svalue);
                            line = line.PostfixThisWithSeperators(countPostFixes, seperator);
                        }
                        ///File.AppendAllText(datFile, line.Trim());
                        sw.WriteLine(line.Trim());
                    }
                }
            }
            catch (CheckedException ex)
            {
                throw new CheckedException(ErrorType.ProcessFailure, "Saving file failed:\n\r:" + ex.Message);
            }
            return lineCount;
        }
        public static int SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, Collection<string> formats,
                                        string seperator)
        {
            int lineCount = 0;
            try
            {
                using (StreamWriter sw = new StreamWriter(datFile))
                {

                    for (int i = 0; i < rows; i++)
                    {
                        string line = "";
                        lineCount++;

                        for (int j = 0; j < colums; j++)
                        {
                            double value = matrix[i][j];
                            string svalue = string.Format(CultureInfo.InvariantCulture, formats[j], value);
                            line += string.Format(CultureInfo.InvariantCulture, "{0}", svalue);
                            line = line + seperator;
                        }
                        ///File.AppendAllText(datFile, line.Trim());
                        sw.WriteLine(line.Trim());
                    }
                }
            }
            catch (CheckedException ex)
            {
                throw new CheckedException(ErrorType.ProcessFailure, "Saving file failed:\n\r:" + ex.Message);
            }
            return lineCount;
        }





    }
}